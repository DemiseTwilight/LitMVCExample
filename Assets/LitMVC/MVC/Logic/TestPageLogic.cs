using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LitMVC {
	public partial class TestPageView : UIView {
		private TestService _service = new TestService();

		private void Awake() {
			Init();
		}

		public void Init() {
			_service.Init();
			StartCoroutine("LoadData");
		}

		private IEnumerator LoadData() {
			while (_service.testDatas.Count > 0) {
				yield return new WaitForSeconds(1);
			}
			Refulsh();
		}

		public void Refulsh() {
			m_test1_text_TMP_Text.text = _service.testDatas[0].text1;
			m_test2_text_TMP_Text.text = _service.testDatas[0].text2;
		}
	}
}
