using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarPreparePanel : BasePanel
{
     public Button btnClose;
    public override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<WarPreparePanel>();
           
        });
    }
}
