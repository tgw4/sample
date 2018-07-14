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
    /// 順番当てクイズ
    /// </summary>
    public class JunbanBinder : AnswerBasePanel {
        public override Format format => Format.Junban;

        [SerializeField] private ChoiceObject[] choiceObjects;
        private List<int> indexStack = new List<int>();

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            for (int i = 0; i < choiceObjects.Length; i++) {
                var index = i;
                var choiceObject = choiceObjects[index];
                var button = choiceObject.Button;

                button.OnClickAsObservable().Subscribe(_ => {
                    choiceObject.Selected.Value = !choiceObject.Selected.Value;
                }).AddTo(button);

                choiceObject.OnSelected.Subscribe(selected => {
                    if (selected) {
                        choiceObject.Select();
                        indexStack.Add(index);

                        // 全ての選択肢を選択したとき解答送信
                        if (indexStack.Count == choiceObjects.Where(x => x.isActiveAndEnabled).Count()) {
                            var answer = new string(indexStack.SelectMany(x => (x + 1).ToString()).ToArray());
                            model.PushAnswer(answer);
                        }
                    } else {
                        choiceObject.Unselect();
                        if (indexStack.Count > 0) {
                            indexStack.Remove(index);
                        }
                    }
                }).AddTo(this);
            }
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            Reset();

            for (int i = 0; i < quiz.Choices.Count; i++) {
                choiceObjects[i].Answer = quiz.Choices[i];
                choiceObjects[i].gameObject.SetActive(true);
            }

            for(int i = quiz.Choices.Count; i < choiceObjects.Length; i++) {
                choiceObjects[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            for (int i = 0; i < choiceObjects.Length; i++) {
                choiceObjects[i].Answer = string.Empty;
                choiceObjects[i].gameObject.SetActive(false);
            }
            indexStack.Clear();
        }
    }
}
