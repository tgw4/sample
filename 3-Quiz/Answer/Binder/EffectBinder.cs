using WdServerAgent.Model.Config;
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

namespace Wisdom {
    /// <summary>
    /// エフェクトクイズ
    /// </summary>
    public class EffectBinder : TypingBinder {
        public override Format format => Format.Effect;

        [SerializeField]
        private Text EffectText;

        // 開始時のスケール
        public const float DefaultScale = 3.0f;

        // エフェクト終了時のスケール
        public const float EndScale = 1.0f;

        // エフェクトを終了する残り時間
        public const float EndAnimationSecond = 5.0f;

        private CompositeDisposable disposable;

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {
            base.Bind(model);
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            base.Initialize(quiz);

            disposable = new CompositeDisposable();

            // 選択肢をエフェクト代わりに使用
            EffectText.text = quiz.Choices[0];

            var elapsedTime = 0.0f;
            this.UpdateAsObservable().Subscribe(_ => {
                elapsedTime += Time.deltaTime;

                // 時間経過によるスケール変化
                var value = Mathf.Lerp(EndScale, DefaultScale, 1 - elapsedTime / (QuizConfig.LimitSeconds - EndAnimationSecond));
                EffectText.transform.localScale = new Vector3(value, value);
            }).AddTo(disposable);
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            EffectText.transform.localScale = new Vector3(DefaultScale, DefaultScale);
            EffectText.text = string.Empty;
            disposable?.Dispose();
            base.Reset();
        }
    }
}
