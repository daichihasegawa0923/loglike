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
        private List<List<Stage>> _stageMap;

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

            for (int i = 0; i < ySize; i++)
            {
                var xList = new List<Stage>(xSize);
                for (int j = 0; j < xSize; j++)
                {
                    // 一度すべて壁にする
                    xList[j] = new Stage(new Vector2(j, i),true,false);
                }
                list[i] = xList;
            }

            this._stageMap = list;
            DigLogic(this._stageMap);
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
                    var randY = (int)Random.Range(1, map.Count - 1);
                    var randX = (int)Random.Range(1, map[randY].Count - 1);
                    stage = map[randY][randX];
                }

                var forward = (int)Random.Range(0, 4);
                switch(forward)
                {
                    case 0:

                        break;
                }
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
                    var right = x + 2 < map[y].Count ? map[y][x + 2].IsWall : false;
                    var left = x - 2 >= 0 ? map[y][x - 2].IsWall : false;
                    var down = y + 2 < map.Count ? map[y + 2][x].IsWall : false;
                    var up = y - 2 >= 0 ? map[y - 2][x].IsWall : false;

                    var canDig = right || left || down || up;
                    if (canDig)
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
        private Stage ChooseDigLogicEachStage(Stage stage, List<List<Stage>> map)
        {
            Stage nextStage = null;
            var kouhoList = new List<Stage>();

            var position = stage.Position;
            var right = position.x + 2 < map[(int)position.y].Count ? map[(int)position.y][(int)position.x + 2].IsWall : false;
            if (right)
                kouhoList.Add(map[(int)position.y][(int)position.x + 1]);

            var left = position.x - 2 >= 0 ? map[(int)position.y][(int)position.x - 2].IsWall : false;
            if (left)
                kouhoList.Add(map[(int)position.y][(int)position.x - 1]);

            var down = position.y + 2 < map.Count ? map[(int)position.y + 2][(int)position.x].IsWall : false;
            if (down)
                kouhoList.Add(map[(int)position.y + 1][(int)position.x]);

            var up = position.y - 2 >= 0 ? map[(int)position.y - 2][(int)position.x].IsWall : false;
            if (up)
                kouhoList.Add(map[(int)position.y - 1][(int)position.x]);

            if (kouhoList.Count == 0)
                return nextStage;

            nextStage = kouhoList[(int)Random.Range(0, kouhoList.Count)];
            return nextStage;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}