using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniFramework.Localization
{
    //[ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Dropdown))]
    [LocalizedBehaviour(typeof(TranslationString))]
    public class LocalizedDropdown : LocalizedBehaviour
    {
        [System.Serializable]
        public class Option
        {
            [TranslationKey]
            public string StringTranslationKey;
        }

        [SerializeField]
        private List<Option> _options;

        /// <summary>
        /// UI组件
        /// </summary>
        private Dropdown _dropdown;

        public List<Option> Options
        {
            get
            {
                if (_options == null)
                    _options = new List<Option>();
                return _options;
            }
            set { _options = value; }
        }

        protected override void OnTranslation(ITranslation translation)
        {
            if (_dropdown == null)
                _dropdown = GetComponent<Dropdown>();

            var optionDatas = _dropdown.options;
            for (var i = 0; i < _options.Count; i++)
            {
                var option = _options[i];

                Dropdown.OptionData optionData;
                if (optionDatas.Count == i)
                {
                    optionData = new Dropdown.OptionData();
                    optionDatas.Add(optionData);
                }
                else
                {
                    optionData = optionDatas[i];
                }

                optionData.text = (string)translation.GetTranslationResult(DataTableName, option.StringTranslationKey);
            }
            if (_options.Count > 0)
            {
                TranslationKey = _options[_dropdown.value].StringTranslationKey;
                _dropdown.captionText.text = (string)translation.GetTranslationResult(DataTableName, TranslationKey);
            }
        }
    }
}