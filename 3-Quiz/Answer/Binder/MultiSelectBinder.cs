using WdServerAgent.Model.Entity.Define;
using WdServerAgent.Model.Entity.ViewModel.Item;
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
    /// マルチセレクトクイズ
    /// </summary>
    public class MultiSelectBinder : AnswerBasePanel {
        public override Format format => Format.MultiSelect;

        [SerializeField] private ChoiceObject[] choiceObjects;
        [SerializeField] private Button okButton;
        private List<int> selectedIndexes = new List<int>();

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            for (int i = 0; i < choiceObjects.Length; i++) {
                var index = i;
                var choiceObject = choiceObjects[index];
                choiceObject.OnSelected.Subscribe(isSelected => {
                    if (isSelected) {
                        choiceObject.Select();
                        selectedIndexes.Add(index);
                    } else {
                        choiceObject.Unselect();
                        selectedIndexes.Remove(index);
                    }
                }).AddTo(choiceObject);

                // 選択・非選択の切り替え
                var button = choiceObjects[index].Button;
                button.OnClickAsObservable().Subscribe(_ => {
                    choiceObject.Selected.Value = !choiceObject.Selected.Value;
                }).AddTo(button);
            }

            // OKボタン
            okButton.OnClickAsObservable().Subscribe(_ => {
            var answer = new string(selectedIndexes.OrderBy(x => x).SelectMany(x => (x + 1).ToString()).ToArray());
                model.PushAnswer(answer);
            }).AddTo(okButton);
        }

        /// <summary>
        /// セレクト問題の初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            Reset();
            for (int i = 0; i < choiceObjects.Length; i++) {
                choiceObjects[i].Answer = quiz.Choices[i];
            }
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            for (int i = 0; i < choiceObjects.Length; i++) {
                choiceObjects[i].Answer = string.Empty;
            }
            selectedIndexes.Clear();
        }
    }
}
