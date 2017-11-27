using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class EnumList {

    public enum Colors {
        Red,
        Yellow,
        Green,
        Blue,
        Grey
    }

    public enum MaskProperties {
        LaserMask,
        LookMask,
        ShieldMask
    }

    public enum ExplosionType {
        Fragment,
        BasicEnemy,
        BigEnemy,
        MuzzleFlash,
        RocketFlash
    }

    public static void ColorInit(Colors myColor, SpriteRenderer mySprite, Sprite[] sprites) {
        switch (myColor) {
            case EnumList.Colors.Red:

                mySprite.sprite = sprites[0];
                break;

            case EnumList.Colors.Yellow:

                mySprite.sprite = sprites[1];
                break;

            case EnumList.Colors.Green:

                mySprite.sprite = sprites[2];
                break;

            case EnumList.Colors.Blue:

                mySprite.sprite = sprites[3];
                break;

            default:

                break;
        }
    }

    public static GameObject FragmentColor(Colors myColor) {
        GameObject fragment = null;

        switch (myColor) {
            case EnumList.Colors.Red:

                fragment = Resources.Load("Fragments/RedFragment") as GameObject;
                break;

            case EnumList.Colors.Yellow:

                fragment = Resources.Load("Fragments/YellowFragment") as GameObject;
                break;

            case EnumList.Colors.Green:

                fragment = Resources.Load("Fragments/GreenFragment") as GameObject;
                break;

            default:

                fragment = Resources.Load("Fragments/RedFragment") as GameObject;
                break;
        }
        return fragment;
    }

    public static GameObject ExplosionColor(Colors myColor) {
        GameObject explosion = null;

        switch (myColor) {
            case EnumList.Colors.Red:

                explosion = Resources.Load("Particles/RedFragmentParticle") as GameObject;
                break;

            case EnumList.Colors.Yellow:

                explosion = Resources.Load("Particles/YellowFragmentParticle") as GameObject;
                break;

            case EnumList.Colors.Green:

                explosion = Resources.Load("Particles/GreenFragmentParticle") as GameObject;
                break;

            case EnumList.Colors.Blue:

                explosion = Resources.Load("Particles/BlueFragmentParticle") as GameObject;
                break;

            default:

                explosion = Resources.Load("Particles/BlueFragmentParticle") as GameObject;
                break;
        }
        return explosion;
    }

    public static Colors RandomColor() {
        Colors randomColor = (Colors)Random.Range(0, 3);
        return randomColor;
    }

    public static GameObject LoadExplosionByType(string color, string type) {
        GameObject newExplosion = null;
        newExplosion = Resources.Load("Particles/" + color + type + "Particle") as GameObject;

        return newExplosion;
    }

    public static void ScaleParticleEffect(GameObject particle, float size) {
        particle.transform.localScale *= size;
        ParticleSystem[] scaledParts = particle.GetComponentsInChildren<ParticleSystem>();

        foreach(ParticleSystem p in scaledParts) {
            p.startSize *= size;
            p.startSpeed *= size;
        }


        //scaledParts.startSize *= size;
        //scaledParts.startSpeed *= size;
    }

    public static GameObject ParticleTrailColor(string color, string type) {
        GameObject particleTrail = null;
        particleTrail = Resources.Load("Particles/" + color + type + "Particle") as GameObject;

        return particleTrail;
    }

    public static int CheckLayer(GameObject check) {
        return check.layer;
    }

    public static bool CheckBoolArray(bool[] checks) {
        bool answer = true;

        for (int i = 0; i < checks.Length; i++) {
            if (checks[i] != true) {
                answer = false;
                break;
            }
        }
        return answer;
    }

    public static int Alternator(int i) {
        return i *= -1;
    }

    public static void InitWeapon(GameObject self, GameObject weaponPrefab, string tag = null, int hardPointLoc = 0, float weaponScale = 1f) {
       

        if (tag == "PrimaryWeaponMount") {
            if(self.transform.FindChild("PrimeWeaponMount") != null) {
                weaponPrefab.transform.SetParent(self.transform.FindChild("PrimeWeaponMount"), false);
            }
            else {
                weaponPrefab.transform.SetParent(self.transform, false);
            }
            
            weaponPrefab.transform.localPosition = Vector2.zero;
        }
        else if (tag == "WeaponHardPoint") {
            weaponPrefab.transform.SetParent(self.GetComponent<Entity>().weaponHardPoints[hardPointLoc], false);
            weaponPrefab.transform.localPosition = Vector2.zero;
        }
        else {
            weaponPrefab.transform.SetParent(self.transform, false);
            weaponPrefab.transform.localPosition = Vector2.zero;
        }

        //weaponPrefab.transform.localScale *= self.GetComponent<Entity>().weaponScaleFactor;
        weaponPrefab.transform.localScale *= weaponScale;
        weaponPrefab.transform.localRotation = Quaternion.identity;
        weaponPrefab.gameObject.tag = "Weapon";
        weaponPrefab.gameObject.layer = self.gameObject.layer;
    }

    public static void ConfirmWeaponLayer(GameObject self, GameObject primary, GameObject special = null) {
        if (primary != null)
            primary.layer = self.layer;

        if (special != null)
            special.layer = self.layer;
    }

    public static bool NearlyEqual(float a, float b) {
        return Mathf.Abs(a - b) < 0.00001f;
    }
}