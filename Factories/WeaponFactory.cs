using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Factories
{
    class WeaponFactory
    {

        #region Fields

        private int currentAmmo,
                    spareAmmo,
                    weaponID,
                    damage,
                    accuracy,
                    range,
                    maxAmmo,
                    bulletSpeed,
                    numOfShots,
                    uniqueid;

        private float reloadDelay,
                      fireRateDelay;

        #endregion

        #region Initialization

        public WeaponFactory(int weaponID, int uniqueid)
        {
            this.weaponID = weaponID;
            this.uniqueid = uniqueid;

            switch (weaponID)
            {
                case 1000:
                    this.currentAmmo = 12;
                    this.spareAmmo = 2;
                    this.damage = 25;
                    this.accuracy = 4;
                    this.range = 1000;
                    this.maxAmmo = 12;
                    this.fireRateDelay = 0.1f;
                    this.reloadDelay = 2.2f;
                    this.bulletSpeed = 12;
                    this.numOfShots = 1;
                    break;
                case 2000:
                    this.currentAmmo = 32;
                    this.spareAmmo = 2;
                    this.damage = 10;
                    this.accuracy = 5;
                    this.range = 1000;
                    this.maxAmmo = 32;
                    this.fireRateDelay = 0.02f;
                    this.reloadDelay = 2.5f;
                    this.bulletSpeed = 12;
                    this.numOfShots = 1;
                    break;
                case 3000:
                    this.currentAmmo = 338;
                    this.spareAmmo = 2;
                    this.damage = 10;
                    this.accuracy = 10;
                    this.range = 1000;
                    this.maxAmmo = 8;
                    this.fireRateDelay = 0.4f;
                    this.reloadDelay = 2.8f;
                    this.bulletSpeed = 15;
                    this.numOfShots = 8;
                    break;
                case 4000:
                    this.currentAmmo = 20;
                    this.spareAmmo = 2;
                    this.damage = 30;
                    this.accuracy = 5;
                    this.range = 1000;
                    this.maxAmmo = 20;
                    this.fireRateDelay = 0.2f;
                    this.reloadDelay = 2.5f;
                    this.bulletSpeed = 15;
                    this.numOfShots = 1;
                    break;
                case 5000:
                    this.currentAmmo = 6;
                    this.spareAmmo = 2;
                    this.damage = 90;
                    this.accuracy = 3;
                    this.range = 1000;
                    this.maxAmmo = 6;
                    this.fireRateDelay = 1.1f;
                    this.reloadDelay = 3.0f;
                    this.bulletSpeed = 15;
                    this.numOfShots = 1;
                    break;
                case 6000:
                    this.currentAmmo = 300;
                    this.spareAmmo = 2;
                    this.damage = 1;
                    this.accuracy = 18;
                    this.range = 200;
                    this.maxAmmo = 300;
                    this.fireRateDelay = 0.1f;
                    this.reloadDelay = 3.0f;
                    this.bulletSpeed = 20000;
                    this.numOfShots = 20;
                    break;
            }
        }

        #endregion

        #region Methods

        public int getID()
        {
            return weaponID;
        }

        public int getUID()
        {
            return uniqueid;
        }

        public int getCurrentAmmo()
        {
            return currentAmmo;
        }

        public int getSpareAmmo()
        {
            return spareAmmo;
        }

        public int getDamage()
        {
            return damage;
        }

        public int getAccuracy()
        {
            return accuracy;
        }

        public int getRange()
        {
            return range;
        }

        public int getMaxAmmo()
        {
            return maxAmmo;
        }

        public float getFireRateDelay() 
        {
            return fireRateDelay;
        }

        public float getReloadDelay()
        {
            return reloadDelay;
        }

        public int getBulletSpeed()
        {
            return bulletSpeed;
        }

        public int getNumOfShots()
        {
            return numOfShots;
        }

        public void setCurrentAmmo(int newAmmo)
        {
            currentAmmo = newAmmo;
        }

        public void setSpareAmmo(int newAmmo)
        {
            spareAmmo = newAmmo;
        }

        public void setDamage(int newDmg)
        {
            damage = newDmg;
        }

        public void setAccuracy(int newAcc)
        {
            accuracy = newAcc;
        }

        public void setRange(int newRange)
        {
            range = newRange;
        }

        public void setMaxAmmo(int newMaxAmmo)
        {
            maxAmmo = newMaxAmmo;
        }

        public void setFireRateDelay(float newFireRateDelay)
        {
            fireRateDelay = newFireRateDelay;
        }

        public void setReloadDelay(float newReloadDelay)
        {
            reloadDelay = newReloadDelay;
        }

        public void setBulletSpeed(int newBulletSpeed)
        {
            bulletSpeed = newBulletSpeed;
        }

        public void setNumOfShots(int newNumOfShots)
        {
            numOfShots = newNumOfShots;
        }

        #endregion

    }
}
