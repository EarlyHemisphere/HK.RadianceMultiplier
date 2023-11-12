using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RadianceMultiplier {
    public class RadianceControl: MonoBehaviour {
        private List<GameObject> absRads = new List<GameObject>();
        private bool flagP2 = false;
        private bool flagP3 = false;
        private bool flagP4 = false;
        private bool flagP5 = false;
        private bool flagDie = false;
        private bool flag2 = false;
        private bool endFlag = false;
        private int damageDealt = 0;

        private void Start() {
            absRads.Add(GameObject.Find("Absolute Radiance"));

            for (int i = 0; i < RadianceMultiplier.localSettings.numRadiances - 1; i++) {
                absRads.Add(Instantiate(absRads[0]));
            }

            On.HealthManager.TakeDamage += KnightHit;
            StartCoroutine(Init());
        }

        private void KnightHit(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            orig(self, hitInstance);
            damageDealt += hitInstance.DamageDealt;
        }

        private IEnumerator Init() {
            yield return new WaitForSeconds(2f);
            
            absRads.ForEach(absRad => absRad.GetComponent<HealthManager>().hp = 999999);

            flagP2 = false;
            flagP3 = false;
            flagP4 = false;
            flagP5 = false;
            flagDie = false;
            flag2 = false;
            endFlag = false;
        }

        private void Update() {
            if (damageDealt >= 400 * absRads.Count && damageDealt < 850 * absRads.Count) {
                if (!flagP2) {
                    absRads.ForEach(absRad => absRad.LocateMyFSM("Phase Control").SetState("Set Phase 2"));
                    flagP2 = true;
                }
            } else if (damageDealt >= 850 * absRads.Count && damageDealt < 1150 * absRads.Count) {
                if (!flagP3) {
                    absRads.ForEach(absRad => absRad.LocateMyFSM("Phase Control").SetState("Set Phase 3"));
                    flagP3 = true;
                }
            } else if (damageDealt >= 1150 * absRads.Count && damageDealt < 1900 * absRads.Count) {
                if (!flagP4) {
                    absRads.ForEach(absRad => absRad.LocateMyFSM("Phase Control").SetState("Stun 1"));
                    flagP4 = true;
                }
            } else if (damageDealt >= 1900 * absRads.Count && damageDealt < 2280 * absRads.Count) {
                if (!flagP5) {
                    absRads.ForEach(absRad => absRad.LocateMyFSM("Phase Control").SetState("Set Ascend"));
                    flagP5 = true;
                }
            } else if (damageDealt >= 2280 * absRads.Count) {
                if (!flagDie) {
                    absRads.ForEach(absRad => absRad.LocateMyFSM("Control").SetState("Check Pos"));
                    flagDie = true;
                }
            }

            if (absRads.Any(absRad => absRad.LocateMyFSM("Control").ActiveStateName == "Arena 2 Start")) {
                if (!flag2) {
                    absRads.ForEach(absRad => absRad.LocateMyFSM("Control").SetState("Arena 2 Start"));
                    flag2 = true;
                }
            }

            if (absRads.Any(absRad => absRad.LocateMyFSM("Control").ActiveStateName == "Final Impact")) {
                if (!endFlag) {
                    absRads.ForEach(absRad => absRad.LocateMyFSM("Control").SetState("Final Impact"));
                    endFlag = true;
                }
            }
        }

        private void OnDestroy() {
            On.HealthManager.TakeDamage -= KnightHit;
            damageDealt = 0;
            absRads.ForEach(Destroy);
            absRads = new List<GameObject>();
            flagP2 = false;
            flagP3 = false;
            flagP4 = false;
            flagP5 = false;
            flagDie = false;
            flag2 = false;
            endFlag = false;
        }
    }
}