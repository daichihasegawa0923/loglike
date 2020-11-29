using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.StageBuilder
{
    /// <summary>
    /// ステージの一単位の定義
    /// </summary>
    public class Stage
    {
        /// <summary>
        /// 移動可能か
        /// </summary>
        public bool IsMovable { set; get; } = true;

        /// <summary>
        /// 壁かどうか
        /// </summary>
        public bool IsWall { set; get; } = false;

        /// <summary>
        /// 座標
        /// </summary>
        public Vector2 Position { set; get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">座標</param>
        /// <param name="isWall">壁かどうか</param>
        /// <param name="isMovable">移動可能か</param>
        public Stage(Vector2 position, bool isWall = false, bool isMovable = true)
        {
            this.Position = position;
            this.IsWall = isWall;
            this.IsMovable = isMovable;

            if (IsWall)
                IsMovable = false;
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public Stage()
        {

        }
    }
}