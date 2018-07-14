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
    /// 文字パネルクイズ
    /// </summary>
    public class PanelBinder : AnswerBasePanel {
        public override Format format => Format.Panel;

        // 回答表示用のタイル
        [SerializeField] private List<TileObject> tiles;

        // プレイヤーが押すパネル
        [SerializeField] private List<ChoiceObject> panels;

        private int currentIndex = 0;
        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            for (int i = 0; i < panels.Count; i++) {
                var index = i;
                var panel = panels[index];

                panel.Button.OnClickAsObservable().Subscribe(_ => {

                    // パネルの文字をタイルに表示
                    tiles[currentIndex].Answer = panel.Answer;
                    currentIndex++;

                    // 全てのタイルに文字が埋まったときに解答送信
                    var activeTiles = tiles.Where(x => x.isActiveAndEnabled);
                    if (currentIndex == activeTiles.Count()) {
                        var answer = new string(tiles.SelectMany(x => x.Answer).ToArray());
                        model.PushAnswer(answer);
                    }
                }).AddTo(panel.Button);
            }
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            Reset();

            var answer = quiz.CorrectAnswer;
            for (int i = 0; i < quiz.CorrectAnswer.Length; i++) {
                var tile = tiles[i];
                tile.gameObject.SetActive(true);
            }

            var dummies = quiz.Choices[0];
            for (int i = 0; i < dummies.Length; i++) {
                var panel = panels[i];
                panel.Answer = $"{dummies[i]}";
                panel.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            for (int i = 0; i < tiles.Count; i++) {
                var tile = tiles[i];
                tile.Answer = string.Empty;
                tile.gameObject.SetActive(false);
            }

            for (int i = 0; i < panels.Count; i++) {
                var panel = panels[i];
                panel.Answer = string.Empty;
                panel.gameObject.SetActive(false);
            }
            currentIndex = 0;
        }
    }
}
