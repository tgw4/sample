using WdServerAgent.Model.Entity.ViewModel.Item;
using WdServerAgent.Model.Service;
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

namespace Wisdom
{
    /// <summary>
    /// クイズ解答部分
    /// </summary>
    public class AnswerPanel : MonoBehaviour
    {
        // 形式に対応したバインダー
        [SerializeField]
        private List<AnswerBasePanel> AnswerPanelList;

        public void Bind(SceneModel model) {

            AnswerPanelList.ForEach(binder => {
                binder.Bind(model);
            });
        }

        /// <summary>
        /// クイズを出題する
        /// </summary>
        public void StartQuiz(QuizEntity quiz) {

            // パネルの初期化
            var panel = AnswerPanelList.Find(p => p.format == quiz.Format);
            if(panel == null) {
                GeLogger.Error($"not found panel. format = {quiz.Format}");
                return;
            }
            panel.Initialize(quiz);

            // 出題パネルの表示
            AnswerPanelList.ForEach(p => p.gameObject.SetActive(p.format == quiz.Format));
        }

        /// <summary>
        /// クイズをリセットする
        /// </summary>
        public void Reset(QuizEntity quiz) {

            // すべてのパネルを非表示に
            AnswerPanelList.ForEach(p => p.gameObject.SetActive(false));

            // 出題に使用したパネルを初期状態へ
            var panel = AnswerPanelList.Find(p => p.format == quiz.Format);
            panel.Reset();
        }
    }
}
