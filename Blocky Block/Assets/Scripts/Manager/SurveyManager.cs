using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Managers
{
    public class SurveyManager : MonoBehaviour
    {
        private static readonly string FORM_URL = "https://docs.google.com/forms/d/e/1FAIpQLSeh5V3uGqVQqWamANr0UbaEjSCAihe2r19kYa5wkYjJMJDuGQ/viewform";
        public void OpenGoogleForm()
        {
            string formUrl = FORM_URL;
            Application.OpenURL(formUrl);
        }
    }
}
