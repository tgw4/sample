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
    /// セレクトクイズ
    /// </summary>
    public class SelectBinder : AnswerBasePanel {
        public override Format format => Format.Select;

        [SerializeField] private ChoiceObject[] choiceObjects;

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            for (int i = 0; i < choiceObjects.Length; i++) {
                var index = i;
                var button = choiceObjects[index].Button;

                // 解答番号を送信
                button.OnClickAsObservable().Subscribe(_ => {
                    var userAnswer = (index + 1).ToString();
                    model.PushAnswer(userAnswer);
                }).AddTo(button);
            }
        }

        /// <summary>
        /// セレクト問題の初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
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
        }
    }
}
