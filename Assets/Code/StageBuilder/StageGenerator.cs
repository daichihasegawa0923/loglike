using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.StageBuilder
{
    public class StageGenerator : MonoBehaviour
    {
        /// <summary>
        /// 壁のオブジェクト
        /// </summary>
        [SerializeField]
        private GameObject _wallObject;

        /// <summary>
        /// ステージサイズの最小値（X軸）
        /// </summary>
        private const int MINIMUM_X = 20;

        /// <summary>
        /// ステージサイズの最小値（Y軸）
        /// </summary>
        private const int MINIMUM_Y = 20;

        /// <summary>
        /// ステージのマップ
        /// </summary>
        /// <remarks>
        /// 1次元目がY軸、2次元目がX軸
        /// </remarks>
        private List<List<Stage>> _stageMap = new List<List<Stage>>();

        // Start is called before the first frame update
        void Start()
        {
            this.Generate(1);
        }

        /// <summary>
        /// ステージマップを生成する
        /// </summary>
        /// <param name="level">ステージのレベル（例えば、階層など）</param>
        public void Generate(int level)
        {
            var xSize = MINIMUM_X + level * 2;
            var ySize = MINIMUM_Y + level * 2;

            var list = new List<List<Stage>>(ySize);
            var gList = new List<List<GameObject>>(ySize);

            for (int i = 0; i < ySize; i++)
            {
                var xList = new List<Stage>(xSize);
                for (int j = 0; j < xSize; j++)
                {
                    // 一度すべて壁にする
                    xList.Add(new Stage(new Vector2(j, i),true,false));
                }

                list.Add(xList);
            }

            this._stageMap = list;
                        
            DigLogic(this._stageMap);
            GenerateObject(this._stageMap);
        }

        /// <summary>
        /// 穴掘り法のロジック
        /// </summary>
        /// <param name="map">マップ</param>
        public void DigLogic(List<List<Stage>> map)
        {
            Stage stage = null;
            while(CanDigLogic(map))
            {
                if(stage == null)
                {
                    var randY = (int)Random.Range(1, map.Count - 2);
                    var randX = (int)Random.Range(1, map[randY].Count - 2);
                    stage = map[randY][randX];
                }

                var next = ChooseDigLogicEachStage(stage, map);

                if(next == null)
                {
                    stage = null;
                    continue;
                }

                next.ForEach(s => { s.IsWall = false; s.IsMovable = true; });

                stage = next[1];
            }
        }

        /// <summary>
        /// 穴掘り法ロジックの終了判定
        /// </summary>
        /// <returns>true:掘れる, false:掘れない</returns>
        private bool CanDigLogic(List<List<Stage>> map)
        {
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    var next = ChooseDigLogicEachStage(map[y][x], map);
                    if (next != null)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 次の穴を掘る場所を選ぶ
        /// </summary>
        /// <param name="stage">現在の位置</param>
        /// <param name="map">全体マップ</param>
        /// <returns>次の穴を掘る場所</returns>
        private List<Stage> ChooseDigLogicEachStage(Stage stage, List<List<Stage>> map)
        {
            List<Stage> nextStage = new List<Stage>();
            var kouhoList = new List<List<Stage>>();

            var position = stage.Position;
            var right = position.x + 2 < map[(int)position.y].Count ? map[(int)position.y][(int)position.x + 2].IsWall : false;
            if (right)
                kouhoList.Add(new List<Stage> { map[(int)position.y][(int)position.x + 1] , map[(int)position.y][(int)position.x + 2] });

            var left = position.x - 2 >= 0 ? map[(int)position.y][(int)position.x - 2].IsWall : false;
            if (left)
                kouhoList.Add(new List<Stage> { map[(int)position.y][(int)position.x - 1] , map[(int)position.y][(int)position.x - 2] });

            var down = position.y + 2 < map.Count ? map[(int)position.y + 2][(int)position.x].IsWall : false;
            if (down)
                kouhoList.Add(new List<Stage> { map[(int)position.y + 2][(int)position.x], map[(int)position.y + 2][(int)position.x] });

            var up = position.y - 2 >= 0 ? map[(int)position.y - 2][(int)position.x].IsWall : false;
            if (up)
                kouhoList.Add(new List<Stage> { map[(int)position.y - 1][(int)position.x], map[(int)position.y - 2][(int)position.x] });

            if (kouhoList.Count == 0)
                return nextStage;

            nextStage = kouhoList[(int)Random.Range(0, kouhoList.Count)];
            return nextStage;
        }

        private void GenerateObject(List<List<Stage>> map)
        {
            foreach(var list in map)
            {
                foreach(var stage in list)
                {
                    if (!stage.IsWall)
                        continue;
                    var obj = GameObject.Instantiate(_wallObject);
                    var position = obj.transform.position;
                    position.x = stage.Position.x;
                    position.z = stage.Position.y;
                    obj.transform.position = position;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}