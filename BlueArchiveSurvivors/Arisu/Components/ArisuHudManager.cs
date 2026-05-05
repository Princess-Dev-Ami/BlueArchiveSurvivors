using BAMod.Arisu.SkillStates.BaseStates;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace BAMod.Arisu.Components
{
    internal class ArisuHudManager : MonoBehaviour
    {
        private TextMeshProUGUI fuel;
        private TextMeshProUGUI damage;
        private TextMeshProUGUI valid;
        private TextMeshProUGUI invalid;
        private TextMeshProUGUI overheat;
        private HUD Parent;
        private ArisuCharacterMain ArisuMain;
        public void OnEnable()
        {
            Parent = transform.root.GetComponentInParent<HUD>();
            fuel = transform.Find("FuelPercentage").GetComponent<TextMeshProUGUI>();
            damage = transform.Find("AnticipatedDamage").GetComponent<TextMeshProUGUI>();
            valid = transform.Find("ValidTarget").GetComponent<TextMeshProUGUI>();
            invalid = transform.Find("NullTarget").GetComponent<TextMeshProUGUI>();
            overheat = transform.Find("Overheating").GetComponent<TextMeshProUGUI>();
            ArisuMain = (ArisuCharacterMain)EntityStateMachine.FindByCustomName(Parent.targetBodyObject, "Body").state;
        }

        public void Update()
        {
            fuel.text = $"{ArisuMain.Fuel}%";
            if (ArisuMain.ReturnCalcDamage() > 0)
            {
                damage.enabled = true;
                damage.text = $"{ArisuMain.ReturnCalcDamage()}";
            }
            else
            {
                damage.enabled = false;
            }
            if (ArisuMain.target.HasValue)
            {
                valid.enabled = ArisuMain.target.Value;
                invalid.enabled = !ArisuMain.target.Value;
            }
            overheat.enabled = ArisuMain.overheat;
        }

    }
}
