using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR
using WeChatWASM;
#endif

public class QualPanel : BasePanel
{
    public Button btnGame;
    public Button btnWeixin;
    public Button btnQQGroup;
    public Button btnClose;
    public override void Init()
    {
        // 关闭面板
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<QualPanel>();
        });
        // 游戏圈 - 打开微信群
        btnWeixin.onClick.AddListener(() =>
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WXPageManager pageManager = WX.CreatePageManager();
            pageManager.Show(new ShowOption
            {
                openlink = "afAsx_7EqL-NdgDCtTkRs1CSqi27OUpzCMdaOgvhM1gApQsn-R7iSQo3VrA5_znKLiW1ZuMHfMp0h_xlDYiRIqKJnkg5jkvmCwisdoOD1BKO2NNtjfzJyPlSolkfbS0kPwjr_M9HL0n1-JLuNjAPlloaMKpJ37h7h9jNpIwMZs19_vgb2ugMTRCoFqf0s9K8syjXV6bOdiITO6GmwrkJ2Hsl02d2R2R_4RhH2ZGy3ZUDfomrnrgEqUnaL39MPc40GR62n0dapHN0Bz1bpjU_lE_PhGxj_2gG-QTUXDHyiJj-lAAwqVHxPVUdeev1dEK3HB4h1KVUwI6f6-HColybqOQ",
            });
#endif
        });
        // 游戏圈 - 打开QQ群
        btnQQGroup.onClick.AddListener(() =>
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WXPageManager pageManager = WX.CreatePageManager();
            pageManager.Show(new ShowOption
            {
                openlink = "afAsx_7EqL-NdgDCtTkRsxraBGvw5duRDqDrgiMd_w0NkPRm2SKTdtjH2RygFlDSgkaPfTAiBKLaeCv84I8vCmK-uKKEC4Q0xEj9SXppZgS6KfM-D6kdCP5S7RMAOzrdwNse2VRv9VphyPKM3TERXzOiKSnrrEnNJ0RUajkOG5NRRZW0jl8WaJj1kE1thPteK3Tz1aPy5XroDobco4ZFDH9Qg7_cf3Nma8Cj_-ZCtnS1og3Mb6q0ogtBVgTrQ8jAVY6ngzmuC6-CDExHcmIICBz-F1eq_sMHvbeKJ193BmI1n6HDAEq5AzKIuxRkSpAdgK7jcgks13um2pTt8jIjZg",
            });
#endif
        });

        // 游戏圈
        btnGame.onClick.AddListener(() =>
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WXPageManager pageManager = WX.CreatePageManager();
            pageManager.Show(new ShowOption
            {
                openlink = "-SSEykJvFV3pORt5kTNpS6XaS2756Ti3nY7_ReKTBQF6rzjc_rsy2-1Xi9AHrSNUKpNd8PRi-XbC7mjDaHMOSLrAuura9BnxXgwI95Pu-0O1hdkZDeZ9XKVGKKh4XxPsFmgAcBaTPKjiULw9lyFN9YTNXR8J9jxQumj0j9oFA6tdQ0VBaIsuffJ_FuoHJq7DctR8FWS03McguReVCSl3CwUV4kuq9_dKOZkmj-FPFFJC-IvT2ct2v9_AylRCINg57c4-DrPh9BXrMJwmoi7iBz8teb0JUgxJLN0KX2k8lAwEg-IUoDlkINGcFzcOQ1JSfjps9RSWq_YDW4eQ2eSAcw",
            });
#endif
        });
    }
}
