using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    public GameObject gunUI;
    public GameObject daggerUI;
    public GameObject potionUI;
    public GameObject beamUI;

    public RawImage[] gunAmmoSlots;
    public RawImage[] daggerAmmoSlots;
    public RawImage[] potionAmmoSlots;
    public Slider beamCooldownSlider;

    public void UpdateSelectedWeaponUI(string weaponName)
    {
        gunUI?.SetActive(weaponName == "Handgun");
        daggerUI?.SetActive(weaponName == "Dagger");
        potionUI?.SetActive(weaponName == "Potion");
        beamUI?.SetActive(weaponName == "Beam");
    }

    public void UpdateAmmo(string weaponName, int ammoLeft, int maxAmmo)
    {
        RawImage[] slots = null;

        if (weaponName == "Handgun") slots = gunAmmoSlots;
        else if (weaponName == "Dagger") slots = daggerAmmoSlots;
        else if (weaponName == "Potion") slots = potionAmmoSlots;

        if (slots == null) return;

        for (int i = 0; i < slots.Length; i++)
            slots[i].enabled = i < ammoLeft;
    }


    public void UpdateBeamCooldown(float fillPercent)
    {
        if (beamCooldownSlider != null)
            beamCooldownSlider.value = Mathf.Clamp01(fillPercent);
    }

}
