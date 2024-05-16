using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShoot
{
    [BepInPlugin("kruumy.AutoShoot", "AutoShoot", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            GameEvents.OnNoteEvent += GameEvents_OnNoteEvent;
        }
        private void GameEvents_OnNoteEvent()
        {
            SpellBook spellBook = (SpellBook)UnityEngine.Object.FindAnyObjectByType(typeof(SpellBook));
            if( spellBook != null && InputManager.Instant.KeyHoldingDownLeftFire() )
            {
                spellBook.ExecuteNextSpell(SpellType.SHOOT, ItemSlot.HandLeft);
            }
            else if ( spellBook != null && InputManager.Instant.KeyHoldingDownRightFire() )
            {
                spellBook.ExecuteNextSpell(SpellType.SHOOT, ItemSlot.HandRight);
            }
        }
    }
}
