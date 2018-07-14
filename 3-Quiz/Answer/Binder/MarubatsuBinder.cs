using WdServerAgent.Model.Entity.Define;
using WdServerAgent.Model.Entity.ViewModel.Item;
using UniRx;
using UnityEngine;

namespace Wisdom {
    /// <summary>
    /// ○×クイズ
    /// </summary>
    public class MarubatsuBinder : AnswerBasePanel {
        public override Format format => Format.Marubatsu;

        [SerializeField] private ChoiceObject[] choiceObjects;

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            for (int i = 0; i < choiceObjects.Length; i++) {
                var index = i;
                var button = choiceObjects[index].Button;
                var answer = choiceObjects[index].Answer;

                // 解答番号を送信
                button.OnClickAsObservable().Subscribe(_ => {
                    model.PushAnswer(answer);
                }).AddTo(button);
            }
        }

        /// <summary>
        /// セレクト問題の初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
        }
    }
}
