using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Wisdom {

    /// <summary>
    /// タイルオブジェクト
    /// </summary>
    public class TileObject : MonoBehaviour {

        [SerializeField] private Text text;
        public string Answer {
            get { return text.text; }
            set { text.text = value; }
        }
    }
}
