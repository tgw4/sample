using WdServerAgent.Model.Entity.Define;
using WdServerAgent.Model.Entity.ViewModel.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiEffect;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Wisdom.Data;

namespace Wisdom {
    public abstract class AnswerBasePanel : DefaultBinder {
        public abstract Format format { get; }

        public override void Bind(SceneModel model) {
            var quizModel = model.FindModel<QuizModel>();
            Bind(quizModel);
        }
        public abstract void Bind(QuizModel model);
        public abstract void Initialize(QuizEntity quiz);
        public virtual void Reset() { }

        /// <summary>
        /// トグルの色を変更させます。
        /// </summary>
        public void ChangeToggleColor(Toggle toggle, int index, bool isOn) {
            var color = toggle.GetComponent<GradientColor>();
            if (isOn) {
                color.colorTop = ColorUtil.CreateColor(255, 255, 170);
                color.colorBottom = ColorUtil.CreateColor(255, 132, 0);
            } else {
                color.colorTop = ColorUtil.CreateColor(255, 255, 255);
                color.colorBottom = ColorUtil.CreateColor(189, 189, 189);
            }
        }
    }
}
