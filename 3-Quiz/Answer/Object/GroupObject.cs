using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Wisdom {

    /// <summary>
    /// グループ分けオブジェクト
    /// </summary>
    public class GroupObject : MonoBehaviour {

        [SerializeField] private Image image;
        [SerializeField] private Text text;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        public Button LeftButton => leftButton;
        public Button RightButton => rightButton;
        private GroupEnum group = GroupEnum.Center;

        // 指定グループにいるときの色
        private Color ColorA = ColorUtil.CreateColor(255, 0, 0);
        private Color ColorB = ColorUtil.CreateColor(0, 0, 255);
        private Color ColorC = ColorUtil.CreateColor(19, 255, 0);

        public string Answer {
            get { return text.text; }
            set { text.text = value; }
        }

        /// <summary>
        /// 左へ移動します。
        /// </summary>
        public void MoveLeft(int groupCount) {
            if (groupCount == 2 || group == GroupEnum.Left) {
                group = GroupEnum.Left;
            } else {
                group = (GroupEnum)(group - 1);
            }
            ChangeColor(groupCount);
        }

        /// <summary>
        /// グループの色に合わせて変化します。
        /// </summary>
        private void ChangeColor(int groupCount) {
            switch (group) {
                case GroupEnum.Left:
                    image.color = ColorA;
                    break;
                case GroupEnum.Center:
                    image.color = groupCount == 2 ? ColorC : ColorB;
                    break;
                case GroupEnum.Right:
                    image.color = groupCount == 2 ? ColorB : ColorC;
                    break;
            }
        }

        /// <summary>
        /// インデックスを返します。
        /// </summary>
        public int? GetIndex(int groupCount) {
            switch (group) {
                case GroupEnum.Left:
                    return 0;
                case GroupEnum.Center:
                    return groupCount == 2 ? (int?)null : 1;
                case GroupEnum.Right:
                    return groupCount == 2 ? 1 : 2;
            }
            return null;
        }

        public string GetAnswer(int groupCount) {
            switch (group) {
                case GroupEnum.Left:
                    return "A";
                case GroupEnum.Center:
                    return groupCount == 2 ? "-" : "B";
                case GroupEnum.Right:
                    return groupCount == 2 ? "B" : "C";
            }
            return "-";
        }

        /// <summary>
        /// 右へ移動します。
        /// </summary>
        public void MoveRight(int groupCount) {
            if (groupCount == 2 || group == GroupEnum.Right) {
                group = GroupEnum.Right;
            } else {
                group = (GroupEnum)(group + 1);
            }
            ChangeColor(groupCount);
        }
    }
}
