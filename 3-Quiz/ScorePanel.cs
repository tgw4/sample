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
    /// スコア部分
    /// </summary>
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField]
        private Image characterImage;

        [SerializeField]
        private Text playerName;

        [SerializeField]
        private Text score;

        [SerializeField]
        private Text rank;

        [SerializeField]
        private Image nextRankBackground;

        [SerializeField]
        private GameObject correctObject;

        [SerializeField]
        private GameObject incorrectObject;

        /// <summary>
        /// スコアを変更します。
        /// </summary>
        public void ChangeScore(int before, int after) {
            if(before < after) {
                var upScore = after - before;
                this.score.text = $"<color=yellow>(+{upScore})</color>{after}点";
            } else {
                this.score.text = $"{after}点";
            }

            var rank = QuizUtil.GetQuizRank(after);
            this.rank.text = rank.ToString();

            var rankRatio = QuizUtil.GetRankRatio(after);
            nextRankBackground.fillAmount = rankRatio;
        }

        /// <summary>
        /// 正誤を表すオブジェクト(○×)を表示します。
        /// </summary>
        public void ActivateFalsehoodObject(bool isCorrect) {
            correctObject.SetActive(isCorrect);
            incorrectObject.SetActive(!isCorrect);
        }
    }
}
