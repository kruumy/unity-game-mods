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

        private bool CanUseSpells()
        {
            return UserInterfaceReferences.Instant.UI_ModeSwapper.IsModeLoaded(UI_Mode.InGame) && !UserInterfaceReferences.Instant.UI_ModeSwapper.IsModeLoaded(UI_Mode.JukeboxLight) && !UserInterfaceReferences.Instant.UI_ModeSwapper.IsModeLoaded(UI_Mode.JukeboxRewind);
        }

        private void GameEvents_OnNoteEvent()
        {
            SpellBook spellBook = (SpellBook)UnityEngine.Object.FindAnyObjectByType(typeof(SpellBook));
            if( spellBook != null && InputManager.Instant.KeyHoldingDownLeftFire() && CanUseSpells() )
            {
                spellBook.ExecuteNextSpell(SpellType.SHOOT, ItemSlot.HandLeft);
            }
            else if ( spellBook != null && InputManager.Instant.KeyHoldingDownRightFire() && CanUseSpells() )
            {
                spellBook.ExecuteNextSpell(SpellType.SHOOT, ItemSlot.HandRight);
            }
        }
    }
}
