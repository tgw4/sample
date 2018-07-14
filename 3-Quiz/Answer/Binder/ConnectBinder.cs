using WdServerAgent.Model.Entity.Define;
using WdServerAgent.Model.Entity.ViewModel.Item;
using WdServerAgent.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Wisdom.Data;

namespace Wisdom {
    /// <summary>
    /// 線結びクイズ
    /// </summary>
    public class ConnectBinder : AnswerBasePanel {
        public override Format format => Format.Connect;

        [SerializeField] private List<ChoiceObject> lefts;
        [SerializeField] private List<ChoiceObject> rights;
        [SerializeField] private List<LineRenderer> lines;
        private Dictionary<LineIndex, LineRenderer> lineMap = new Dictionary<LineIndex, LineRenderer>();

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {
            for(int index = 0; index < lefts.Count; index++) {
                Bind(model, index, true);
            }

            for (int index = 0; index < rights.Count; index++) {
                Bind(model, index, false);
            }
        }

        /// <summary>
        /// オブジェクトをバインドします。
        /// </summary>
        private void Bind(QuizModel model, int index, bool isLeft) {
            var designatedObject = isLeft ? lefts[index] : rights[index];
            var button = designatedObject.Button;

            var sameSideObjects = isLeft ? lefts : rights;
            var oppositeSideObjects = isLeft ? rights : lefts;

            button.OnClickAsObservable().Subscribe(_ => {

                // 同一サイドが選択済み
                var selectedSameSideObject = sameSideObjects.Find(x => x.Selected.Value);
                if (selectedSameSideObject != null) {
                    selectedSameSideObject.Selected.Value = false;
                    designatedObject.Selected.Value = true;
                    return;
                }

                // 反対サイドが選択済み
                var selectedOppositeSideObject = oppositeSideObjects.Find(x => x.Selected.Value);
                if (selectedOppositeSideObject != null) {
                    selectedOppositeSideObject.Selected.Value = false;
                    var selectedIndex = oppositeSideObjects.IndexOf(selectedOppositeSideObject);

                    var leftIndex = isLeft ? index : selectedIndex;
                    var rightIndex = isLeft ? selectedIndex : index;
                    DrawLine(leftIndex, rightIndex);
                    if(lineMap.Count >= lefts.Where(x => x.isActiveAndEnabled).Count()) {
                        var answer = new string(lineMap.Keys.OrderBy(x => x.left).SelectMany(x => (x.right + 1).ToString()).ToArray());
                        model.PushAnswer(answer);
                    }
                    return;
                }

                // 選択状態にする
                designatedObject.Selected.Value = true;

            }).AddTo(button);

            // 選択オブジェクト
            designatedObject.OnSelected.Subscribe(selected => {
                if (selected) {
                    designatedObject.Select();
                } else {
                    designatedObject.Unselect();
                }
            }).AddTo(this);
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            Reset();

            var countOfChoices = quiz.Choices.Count / 2;
            var leftChoices = quiz.Choices.Take(countOfChoices).ToList();
            var rightChoices = quiz.Choices.Skip(countOfChoices).ToList();

            for (int i = 0; i < lefts.Count && i < leftChoices.Count; i++) {
                var left = lefts[i];
                left.Answer = leftChoices[i];
                left.gameObject.SetActive(true);
            }

            for (int i = 0; i < rights.Count && i < rightChoices.Count; i++) {
                var right = rights[i];
                right.Answer = rightChoices[i];
                right.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            foreach (var left in lefts) {
                left.Answer = string.Empty;
                left.gameObject.SetActive(false);
            }

            foreach (var right in rights) {
                right.Answer = string.Empty;
                right.gameObject.SetActive(false);
            }

            foreach(var line in lines) {
                line.enabled = false;
            }
            lineMap.Clear();
        }

        /// <summary>
        /// 線を引きます。
        /// </summary>
        public void DrawLine(int leftIndex, int rightIndex) {

            GeLogger.Debug($"DrawLine {leftIndex}-{rightIndex}");

            // すでに引いている線の削除
            var drawedLines = lineMap.Keys.Where(x => x.left == leftIndex || x.right == rightIndex).Distinct().ToList();
            foreach(var drawedLine in drawedLines) {
                lineMap[drawedLine].enabled = false;
                lineMap.Remove(drawedLine);
            }

            // 線を引く
            LineRenderer renderer = lines[leftIndex];
            renderer.enabled = true;
            var leftPos = lefts[leftIndex].transform.position;
            var rightPos = rights[rightIndex].transform.position;
            renderer.SetPosition(0, new Vector3(leftPos.x, leftPos.y, -1));
            renderer.SetPosition(1, new Vector3(rightPos.x, rightPos.y, -1));

            lineMap.Add(new LineIndex(leftIndex, rightIndex), renderer);
        }

        public class LineIndex {
            public int left;
            public int right;

            public LineIndex(int left, int right) {
                this.left = left;
                this.right = right;
            }

            public override bool Equals(object obj) {
                //objがnullか、型が違うときは、等価でない
                if (obj == null || this.GetType() != obj.GetType()) {
                    return false;
                }
                //この型が継承できないクラスや構造体であれば、次のようにできる
                //if (!(obj is TestClass))

                //Numberで比較する
                LineIndex c = (LineIndex)obj;
                return (this.left == c.left && this.right == c.right);
            }

            public override int GetHashCode() {
                return this.left + this.right;
            }
        }
    }
}
