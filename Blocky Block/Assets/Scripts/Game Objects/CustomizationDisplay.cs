using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace BlockyBlock
{
    public class CustomizationDisplay : MonoBehaviour
    {
        public GameObject[] Bodies;
        public GameObject[] BodyParts;
        public GameObject[] Eyes;
        public GameObject[] Gloves;
        public GameObject[] Mouths;
        public GameObject[] Noses;
        public GameObject[] Ears;
        public GameObject[] Glasses;
        public GameObject[] Hairs;
        public GameObject[] Hats;
        public GameObject[] Horns;
        public GameObject[] Tails;
        public void Setup(
            int _body = 0, 
            int _bodyPart = -1, 
            int _eyes = 0, 
            int _gloves = -1, 
            int _mouth = 0, 
            int _nose = -1, 
            int _ears = -1, 
            int _glasses = -1, 
            int _hair = -1, 
            int _hat = -1, 
            int _horn = -1, 
            int _tail = -1)
        {
            SetBody(_body);
            SetBodyPart(_bodyPart);
            SetEyes(_eyes);
            SetGloves(_gloves);
            SetMouth(_mouth);
            SetNose(_nose);
            SetEars(_ears);
            SetGlasses(_glasses);
            SetHair(_hair);
            SetHat(_hat);
            SetHorn(_horn);
            SetTail(_tail);
        }
        public void SetBody(int _idx = 0)
        {
            for (int i = 0; i < Bodies.Length; i++)
            {
                if (Bodies[i].activeSelf == (i == _idx)) continue;
                Bodies[i].SetActive(i == _idx);
            }
        }
        public void SetBodyPart(int _idx = -1)
        {
            for (int i = 0; i < BodyParts.Length; i++)
            {
                if (BodyParts[i].activeSelf == (i == _idx)) continue;
                BodyParts[i].SetActive(i == _idx);
            }
        }
        public void SetEyes(int _idx = 0)
        {
            for (int i = 0; i < Eyes.Length; i++)
            {
                if (Eyes[i].activeSelf == (i == _idx)) continue;
                Eyes[i].SetActive(i == _idx);
            }
        }
        public void SetGloves(int _idx = 0)
        {
            for (int i = 0; i < Gloves.Length; i++)
            {
                if (Gloves[i].activeSelf == (i == _idx)) continue;
                Gloves[i].SetActive(i == _idx);
            }
        }
        public void SetMouth(int _idx = 0)
        {
            for (int i = 0; i < Mouths.Length; i++)
            {
                if (Mouths[i].activeSelf == (i == _idx)) continue;
                Mouths[i].SetActive(i == _idx);
            }
        }
        public void SetNose(int _idx = 0)
        {
            for (int i = 0; i < Noses.Length; i++)
            {
                if (Noses[i].activeSelf == (i == _idx)) continue;
                Noses[i].SetActive(i == _idx);
            }
        }
        public void SetEars(int _idx = 0)
        {
            for (int i = 0; i < Ears.Length; i++)
            {
                if (Ears[i].activeSelf == (i == _idx)) continue;
                Ears[i].SetActive(i == _idx);
            }
        }
        public void SetGlasses(int _idx = 0)
        {
            for (int i = 0; i < Glasses.Length; i++)
            {
                if (Glasses[i].activeSelf == (i == _idx)) continue;
                Glasses[i].SetActive(i == _idx);
            }
        }
        public void SetHair(int _idx = 0)
        {
            for (int i = 0; i < Hairs.Length; i++)
            {
                if (Hairs[i].activeSelf == (i == _idx)) continue;
                Hairs[i].SetActive(i == _idx);
            }
        }
        public void SetHat(int _idx = 0)
        {
            for (int i = 0; i < Hats.Length; i++)
            {
                if (Hats[i].activeSelf == (i == _idx)) continue;
                Hats[i].SetActive(i == _idx);
            }
        }
        public void SetHorn(int _idx = 0)
        {
            for (int i = 0; i < Horns.Length; i++)
            {
                if (Horns[i].activeSelf == (i == _idx)) continue;
                Horns[i].SetActive(i == _idx);
            }
        }
        public void SetTail(int _idx = 0)
        {
            for (int i = 0; i < Tails.Length; i++)
            {
                if (Tails[i].activeSelf == (i == _idx)) continue;
                Tails[i].SetActive(i == _idx);
            }
        }
        public int GetCustomizationCountByType(CustomizationType type)
        {
            switch (type)
            {
                case CustomizationType.BODY:
                    return this.Bodies.Length;
                case CustomizationType.BODY_PART:
                    return this.BodyParts.Length;
                case CustomizationType.EARS:
                    return this.Ears.Length;
                case CustomizationType.EYES:
                    return this.Eyes.Length;
                case CustomizationType.GLASSES:
                    return this.Glasses.Length;
                case CustomizationType.GLOVES:
                    return this.Gloves.Length;
                case CustomizationType.HAIR:
                    return this.Hairs.Length;
                case CustomizationType.HAT:
                    return this.Hats.Length;
                case CustomizationType.HORN:
                    return this.Horns.Length;
                case CustomizationType.MOUTH:
                    return this.Mouths.Length;
                case CustomizationType.NOSE:
                    return this.Noses.Length;
                case CustomizationType.TAIL:
                    return this.Tails.Length;
                default:
                    return 0;
            }
        }
    }
}
