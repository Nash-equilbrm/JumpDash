using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
    public enum EventID
    {
        #region Gameplay
        InitGamePlay,
        PlayerFinishMovement,
        GainPoint,
        HitBlock,
        BackToMainMenu,
        #endregion

        #region UI
        OnPlayBtnClicked,
        OnBackToMainMenuBtnClicked,
        #endregion
    }
}
