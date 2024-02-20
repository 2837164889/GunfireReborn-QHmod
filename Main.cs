using MelonLoader;
using UnityEngine;
using System;
using Il2CppSystem.Collections.Generic;
using HeroCameraName;
using Item;
using DataHelper;
using MelonLoader.Modules;
using Il2CppSystem.IO;
using System.Diagnostics;
using Steamworks;
using ClientCollet;
using System.Linq;
using UnhollowerBaseLib;

namespace QHMod
{
    public static class BuildInfo
    {
        public const string Name = "QHMod-枪火重生模组"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "枪火重生模组.原作者pentium1131和Hkl146 zhang.由修改"; // Description for the Mod.  (Set as null if none)
        public const string Author = "RanDomHacker"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "2.7.3"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "https://github.com/2837164889/GunfireReborn-QHmod"; // Download Link for the Mod.  (Set as null if none)
        public const string Discord = "https://discord.gg/xkXmc5EwSx"; //Discord Link for Mod. (Set as null if none)
    }

    public class QHMod : MelonMod
    {
        public static bool HelpText = false;
        public static KeyCode HelpTextkey = KeyCode.End;
        public static bool shownpc = false;
        public static KeyCode showUIKey = KeyCode.Home;
        public static KeyCode needinitKey = KeyCode.Insert;
        public static bool showUI = false;
        public static KeyCode autoaimKey = KeyCode.F1;
        public static bool autoaim = false;
        public static KeyCode weaponEnhanceKey = KeyCode.F2;
        public static bool weaponEnhance = false;
        public static KeyCode playerEnhanceKey = KeyCode.F3;
        public static bool playerEnhance = false;
        public static KeyCode AimKey = KeyCode.F4;
        public static bool Aim = false;
        public static KeyCode AimKey1 = KeyCode.F9;
        public static bool Aim1 = false;
        public static KeyCode AttDistanceStateKey = KeyCode.F5;
        public static bool AttDistanceState = false;
        public static KeyCode SuperJumpTypeKey = KeyCode.F6;
        public static bool SuperJumpType = false;
        public static KeyCode shownpcKey = KeyCode.LeftAlt;
        public static KeyCode ShowBloodBarStateKey = KeyCode.X;
        public static bool ShowBloodBarState = false;
        public static KeyCode pickupKey = KeyCode.Mouse2;
        public static KeyCode ZoomWeakStateKey = KeyCode.Z;
        public static bool ZoomWeakState = false;
        public static float ZoomWeakNum = 2.5f;
        private float originJumpHeight;
        private float originSpeed;
        public static bool needinit = true;
        public static KeyCode DiscordKey = KeyCode.PageDown;
        public static KeyCode GithubKey = KeyCode.PageUp;
        

        /**
         * 快捷键开关相关代码
         */
        public void SwitchHotKey()
        {
            // 重新初始化开关
            if (Input.GetKeyUp(needinitKey))
            {
                needinit = !needinit;
            }
            // 辅助瞄准开关
            if (Input.GetKeyUp(autoaimKey))
            {
                autoaim = !autoaim;
                MelonLogger.Msg("辅助瞄准已" + (autoaim ? "开启" : "关闭"));
            }
            // 显示UI开关
            if (Input.GetKeyUp(showUIKey))
            {
                showUI = !showUI;
                MelonLogger.Msg("UI已" + (showUI ? "开启" : "关闭"));
            }
            // 武器增强开关
            if (Input.GetKeyUp(weaponEnhanceKey))
            {
                weaponEnhance = !weaponEnhance;
                MelonLogger.Msg("武器增强已" + (weaponEnhance ? "开启" : "关闭"));
            }
            // 子弹跟踪
            if (Input.GetKeyUp(QHMod.AimKey))
            {
                if (QHMod.Aim1)
                {
                    QHMod.Aim1 = false;
                    QHMod.Aim = false;
                    MelonLogger.Msg("子弹跟踪已关闭");
                }
                else
                {
                    QHMod.Aim = !QHMod.Aim;
                    MelonLogger.Msg("子弹跟踪已" + (QHMod.Aim ? "开启" : "关闭"));
                }
            }
            //子弹跟踪爆炸专用
            if (Input.GetKeyUp(QHMod.AimKey1))
            {
                if (QHMod.Aim)
                {
                    QHMod.Aim = false;
                    MelonLogger.Msg("子弹跟踪已关闭");
                }
                else
                {
                    QHMod.Aim1 = !QHMod.Aim1;
                    MelonLogger.Msg("爆炸专用子弹跟踪已" + (QHMod.Aim1 ? "开启" : "关闭"));
                }
            }
            // 透视开关
            if (Input.GetKey(shownpcKey))
                shownpc = true;
            else
                shownpc = false;
            // 玩家增强开关
            if (Input.GetKeyUp(playerEnhanceKey))
            {
                playerEnhance = !playerEnhance;
                MelonLogger.Msg("玩家增强已" + (playerEnhance ? "开启" : "关闭"));
            }
            if (Input.GetKeyUp(ShowBloodBarStateKey))
            {
                ShowBloodBarState = !ShowBloodBarState;
                MelonLogger.Msg("血条透视已" + (ShowBloodBarState ? "开启" : "关闭"));
            }
            //近战距离
            if (Input.GetKeyDown(AttDistanceStateKey))
            {
                AttDistanceState = !AttDistanceState;
                MelonLogger.Msg("近战距离已" + (AttDistanceState ? "开启" : "关闭"));
            }
            if (Input.GetKeyUp(SuperJumpTypeKey))
            {
                SuperJumpType = !SuperJumpType;
                MelonLogger.Msg("起飞已" + (SuperJumpType ? "开启" : "关闭"));
            }
            if (Input.GetKeyUp(ZoomWeakStateKey))
            {
                ZoomWeakState = !ZoomWeakState;
                MelonLogger.Msg("大头开关已" + (ZoomWeakState ? "开启" : "关闭"));
            }
            //鼠标滚轮中键吸物
            if (Input.GetKeyDown(pickupKey))
            {
                /*
                 * (ServerDefine.FightType)16777225 && Shape 5504 魂
                 * NWARRIOR_DROP_CASH 钱
                 * NWARRIOR_DROP_BULLET 子弹 次技能
                 * NWARRIOR_DROP_EQUIP 武器
                 * NWARRIOR_DROP_RELIC 卷轴
                 * NWARRIOR_NPC_GOLDENCUP 金爵
                 * NWARRIOR_DROP_TRIGGER && Shape 5513 包子
                 * keyValuePair.Value.DropOPCom.FlyMeToTheMoon(HeroCameraManager.HeroID); 有BUG
                 * 东西过多 使用 修改AutoPickRange属性 会造成卡顿
                 */
                foreach (var keyValuePair in NewPlayerManager.PlayerDict)
                {
                    if (keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_BULLET || keyValuePair.Value.Shape == 5513)
                    {
                        keyValuePair.Value.DropOPCom.AutoPickRange = 99999f;
                        continue;
                    }
                    if (keyValuePair.Value.Shape == 5504 || keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_CASH)
                    {
                        Vector3 HeroPosition = HeroCameraManager.HeroTran.position;
                        keyValuePair.Value.gameTrans.position = new Vector3(HeroPosition.x, HeroPosition.y, HeroPosition.z);
                        continue;
                    }
                    if (keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_EQUIP
                       || keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_RELIC
                       || keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_NPC_GOLDENCUP)
                    {
                        Ray ray = new Ray(CameraManager.MainCamera.position, CameraManager.MainCamera.forward);
                        Vector3 position = ray.GetPoint(3);
                        keyValuePair.Value.gameTrans.position = new Vector3(position.x + GetRandomNumber(-2, 2), position.y, position.z + GetRandomNumber(-2, 2));
                    }
                    MelonLogger.Msg("已捡取物品");
                }
            }
            //打开浏览器加入discord
            if (Input.GetKeyDown(DiscordKey))
            {
               String url =  BuildInfo.Discord.ToString();
                Process.Start(url);
            }
            //打开github项目
            if (Input.GetKeyDown(GithubKey)) 
            {
                String url = BuildInfo.DownloadLink.ToString();
                Process.Start(url);
            }
        }
        public override void OnUpdate()
        {
            try
            {
                // 初始化
                if ((needinit) && HeroCameraManager.HeroObj != null && HeroCameraManager.HeroObj.BulletPreFormCom != null && HeroCameraManager.HeroObj.BulletPreFormCom.weapondict != null)
                {
                    shownpc = false;
                    showUI = false;
                    autoaim = false;
                    Aim = false;
                    Aim1 = false;
                    AttDistanceState = false;
                    weaponEnhance = false;
                    playerEnhance = false;
                    needinit = false;
                    SuperJumpType = false;
                    ZoomWeakState = false;
                }
                // 设置快捷键
                SwitchHotKey();
                // 武器增强
                if (weaponEnhance && HeroCameraManager.HeroObj != null && HeroCameraManager.HeroObj.BulletPreFormCom != null && HeroCameraManager.HeroObj.BulletPreFormCom.weapondict != null)
                {
                    foreach (KeyValuePair<int, WeaponPerformanceObj> weapon in HeroCameraManager.HeroObj.BulletPreFormCom.weapondict)
                    {
                        // 弹药
                        weapon.value.ModifyBulletInMagzine(200, 200);
                        weapon.value.ReloadBulletImmediately();
                        //武器精确度
                        if (weapon.value.WeaponAttr.Stability[0] != 10000)
                        {
                            weapon.value.WeaponAttr.Stability[0] = 100000;
                        }
                        //武器稳定性
                        if (weapon.value.WeaponAttr.Accuracy[0] != 10000)
                        {
                            weapon.value.WeaponAttr.Accuracy[0] = 100000;
                        }
                        // 穿透 效果存疑
                        weapon.value.WeaponAttr.Pierce = 100;
                        //爆炸范围(会影响爆炸类武器、火标和电手套)
                        if (weapon.value.WeaponAttr.Radius > 0f && weapon.value.WeaponAttr.Radius < 9f)
                        {
                            weapon.value.WeaponAttr.Radius = 9f;
                        }
                        // 射程
                        if (weapon.value.WeaponAttr.AttDis != 9999f)
                        {
                            weapon.value.WeaponAttr.AttDis = 9999f;
                        }
                        // 换弹时间
                        if (weapon.value.WeaponAttr.FillTime[0] != 5)
                        {
                            weapon.value.WeaponAttr.FillTime[0] = 5;
                        }
                        // 子弹速度
                        if (weapon.value.WeaponAttr.BulletSpeed >= 50f && weapon.value.WeaponAttr.BulletSpeed != 55f || weapon.value.WeaponAttr.BulletSpeed == 30f)
                        {
                            if (weapon.value.WeaponAttr.BulletSpeed != 100f)
                            {
                                weapon.value.WeaponAttr.BulletSpeed = 500f;
                            }
                        }
                        else if (weapon.value.WeaponAttr.BulletSpeed == 30f && weapon.value.WeaponAttr.BulletSpeed != 60f)
                        {
                            weapon.value.WeaponAttr.BulletSpeed = 200f;
                        }
                    }
                }
                // 玩家增强
                if (HeroCameraManager.HeroObj != null)
                {
                    if (HeroCameraManager.HeroObj != null)
                    {
                        if (originJumpHeight == 0f)
                        {
                            originJumpHeight = HeroMoveManager.HMMJS.jumping.baseHeight;
                        }
                        if (originSpeed == 0f)
                        {
                            originSpeed = HeroMoveManager.HMMJS.maxForwardSpeed;
                        }

                        // 计算增强状态下的速度和跳跃高度
                        float enhancedJumpHeight = 64f / (HeroMoveManager.HMMJS.movement.gravity * 2f);
                        float enhancedSpeed = 10f;

                        // 根据条件设置速度和跳跃高度
                        if (playerEnhance)
                        {
                            HeroMoveManager.HMMJS.jumping.baseHeight = Mathf.Max(HeroMoveManager.HMMJS.jumping.baseHeight, enhancedJumpHeight);
                            HeroMoveManager.HMMJS.maxForwardSpeed = (HeroMoveManager.HMMJS.maxBackwardsSpeed = (HeroMoveManager.HMMJS.maxSidewaysSpeed = Mathf.Max(HeroMoveManager.HMMJS.maxForwardSpeed, enhancedSpeed)));
                        }
                        else
                        {
                            // 如果不处于增强状态，恢复原始速度和跳跃高度
                            HeroMoveManager.HMMJS.jumping.baseHeight = originJumpHeight;
                            HeroMoveManager.HMMJS.maxForwardSpeed = (HeroMoveManager.HMMJS.maxBackwardsSpeed = (HeroMoveManager.HMMJS.maxSidewaysSpeed = originSpeed));
                        }
                    }
                    /* 以下功能覆盖速度和跳跃
                    if (originJumpHeight == 0f)
                    {
                        originJumpHeight = HeroMoveManager.HMMJS.jumping.baseHeight;
                    }
                    if (originSpeed == 0f)
                    {
                        originSpeed = HeroMoveManager.HMMJS.maxForwardSpeed;
                    }
                    if (playerEnhance)
                    {
                        HeroMoveManager.HMMJS.jumping.baseHeight = 64f / (HeroMoveManager.HMMJS.movement.gravity * 2f);
                        HeroMoveManager.HMMJS.maxForwardSpeed = (HeroMoveManager.HMMJS.maxBackwardsSpeed = (HeroMoveManager.HMMJS.maxSidewaysSpeed = 10f));
                    }
                    else
                    {
                        HeroMoveManager.HMMJS.jumping.baseHeight = originJumpHeight;
                        HeroMoveManager.HMMJS.maxForwardSpeed = (HeroMoveManager.HMMJS.maxBackwardsSpeed = (HeroMoveManager.HMMJS.maxSidewaysSpeed = originSpeed));
                    }*/
                }
                // 辅助瞄准
                if (autoaim && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
                {
                    List<NewPlayerObject> monsters = NewPlayerManager.GetMonsters();
                    if (monsters != null)
                    {

                        Vector3 campos = CameraManager.MainCamera.position;
                        Transform nearmons = null;
                        Transform monsterTransform = null;
                        float neardis = 500f;

                        foreach (var monster in monsters)
                        {

                            if (monster.BodyPartCom == null)
                                continue;

                            if (monster.BloodBarCom == null)
                                continue;

                            if (monster.BloodBarCom.BloodBar == null)
                                continue;

                            monsterTransform = monster.BodyPartCom.GetWeakTrans();

                            if (monsterTransform == null)
                                continue;

                            Vector3 vector = CameraManager.MainCameraCom.WorldToViewportPoint(monsterTransform.position);

                            bool flag = vector.x >= 0.45f && vector.x <= 0.55f;
                            flag = flag && vector.y >= 0.45f && vector.y <= 0.55f;
                            flag = flag && vector.z > 0f;
                            if (flag)
                            {
                                // 计算屏幕角度
                                vector.y = 0f;
                                vector.x = Screen.width * (0.5f - vector.x);
                                vector.z = 0f;
                            }
                            else
                                continue;

                            vector = monsterTransform.position - campos;
                            vector.y += 1.2f;
                            float curdis = vector.magnitude;
                            var hits = Physics.RaycastAll(new Ray(campos, vector), curdis);
                            bool visible = true;

                            foreach (RaycastHit raycastHit in hits)
                            {
                                if (raycastHit.collider.gameObject.layer == 0 || raycastHit.collider.gameObject.layer == 30 || raycastHit.collider.gameObject.layer == 31)
                                {
                                    visible = false;
                                    break;
                                }
                            }

                            if (visible && curdis < neardis)
                            {
                                neardis = curdis;
                                nearmons = monsterTransform;
                            }

                        }
                        // 修改玩家摄像机角度并且瞄准
                        if (nearmons != null)
                        {
                            Vector3 objpos = default;
                            objpos.x = HeroCameraManager.HeroObj.gameTrans.position.x;
                            objpos.y = nearmons.position.y + 0.2f;
                            objpos.z = HeroCameraManager.HeroObj.gameTrans.position.z;
                            Vector3 forward = nearmons.position - objpos;
                            forward.y += 0.12f;
                            Quaternion rotation = Quaternion.LookRotation(forward);
                            HeroCameraManager.HeroObj.gameTrans.rotation = rotation;
                            forward = nearmons.position - campos;
                            forward.y += 0.12f;
                            Quaternion rotation2 = Quaternion.LookRotation(forward);
                            CameraManager.MainCamera.rotation = rotation2;
                        }
                    }
                }
               
                //子弹跟踪
                if (Aim && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
                {
                    List<NewPlayerObject> monsters = NewPlayerManager.GetMonsters();
                    if (monsters == null)
                    {
                        return;
                    }
                    Vector3 position = CameraManager.MainCamera.position;
                    Transform transform = null;
                    float SightRange = 9999999;
                    foreach (var monster in monsters)
                    {   // 无敌怪
                        if (monster.playerProp.HP == 1 && (monster.BloodBarCom.BloodBar.isSpecialUndieChallenge || monster.BloodBarCom.BloodBar.isUndieChallenge || monster.BloodBarCom.BloodBar.isUndieStart))
                        {
                            continue;
                        }
                        try
                        {
                            Transform weakTrans = monster.BodyPartCom.GetWeakTrans(false);
                            if (weakTrans != null)
                            {
                                Vector3 vector = weakTrans.position - position;
                                float Distance = vector.magnitude;
                                Ray ray = new Ray(position, vector);
                                var hits = Physics.RaycastAll(ray, Distance);
                                if (hits.Any(hit => hit.collider.gameObject.tag == "Monster_Shield"))
                                {
                                    ZoomShield();
                                }
                                bool query = hits.Any(hit => hit.collider.gameObject.layer == 0 || hit.collider.gameObject.layer == 30 || hit.collider.gameObject.layer == 31 || hit.collider.gameObject.tag == "Monster_Shield");
                                if (!query && Distance < SightRange)
                                {
                                    SightRange = Distance;
                                    transform = weakTrans;
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (transform != null)
                    {
                        Vector3 forward = transform.position - position;
                        forward.y += 0.14f;
                        Quaternion rotation = Quaternion.LookRotation(forward);
                        CameraManager.MainCamera.rotation = rotation;
                    }
                }

                if (QHMod.Aim1)
                {
                    List<NewPlayerObject> monsters3 = NewPlayerManager.GetMonsters();
                    if (monsters3 != null)
                    {
                        Vector3 position3 = CameraManager.MainCamera.position;
                        Transform transform = null;
                        float num5 = 9999999f;
                        foreach (NewPlayerObject newPlayerObject3 in monsters3)
                        {
                            if (newPlayerObject3.playerProp.HP != 1f || (!newPlayerObject3.BloodBarCom.BloodBar.isSpecialUndieChallenge && !newPlayerObject3.BloodBarCom.BloodBar.isUndieChallenge && !newPlayerObject3.BloodBarCom.BloodBar.isUndieStart))
                            {
                                try
                                {
                                    Transform weakTrans2 = newPlayerObject3.BodyPartCom.GetWeakTrans(false);
                                    if (weakTrans2 != null)
                                    {
                                        Vector3 direction2 = weakTrans2.position - position3;
                                        float magnitude3 = direction2.magnitude;
                                        Il2CppStructArray<RaycastHit> source2 = Physics.RaycastAll(new Ray(position3, direction2), magnitude3);
                                        if (source2.Any((RaycastHit hit) => hit.collider.gameObject.tag == "Monster_Shield"))
                                        {
                                            this.ZoomShield();
                                        }
                                        if (!source2.Any((RaycastHit hit) => hit.collider.gameObject.layer == 0 || hit.collider.gameObject.layer == 30 || hit.collider.gameObject.layer == 31 || hit.collider.gameObject.tag == "Monster_Shield") && magnitude3 < num5)
                                        {
                                            num5 = magnitude3;
                                            transform = weakTrans2;
                                        }
                                    }
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                        if (transform != null)
                        {
                            Vector3 forward3 = transform.position - position3;
                            forward3.y += 0.14f;
                            Quaternion rotation4 = Quaternion.LookRotation(forward3);
                            CameraManager.MainCamera.rotation = rotation4;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MelonLogger.Msg("异常:" + ex.ToString());
            }
        }

        void ZoomShield()
        {
            var Shields = GameObject.FindGameObjectsWithTag("Monster_Shield");
            foreach (var Shield in Shields)
            {
                Shield.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
        public override void OnGUI()
        {
            if (showUI)
            {
                GUILayout.BeginArea(new Rect(0f, 200f, 150f, 500f));
                GUILayout.Label("<b><color=red>"+"QHMod功能菜单"+ BuildInfo.Version.ToString()+ "版本" +"</color></b>", null);
                GUILayout.Label("<color=red>自动捡物品(鼠标中键)</color>", null);
                GUILayout.Label("<b><color=red>显示/隐藏(" + QHMod.showUIKey.ToString() + ")</color></b>", null);
                GUILayout.Label("<b><color=red>重新初始化(" + QHMod.needinitKey.ToString() + ")</color></b>", null);
                GUILayout.Label("<b><color=red>辅助瞄准(" + QHMod.autoaimKey.ToString() + ")：</color></b>" + (QHMod.autoaim ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>武器增强(" + QHMod.weaponEnhanceKey.ToString() + ")：</color></b>" + (QHMod.weaponEnhance ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>玩家增强(" + QHMod.playerEnhanceKey.ToString() + ")：</color></b>" + (QHMod.playerEnhance ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>子弹跟踪(" + QHMod.AimKey.ToString() + ")：</color></b>" + (QHMod.Aim ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>子弹跟踪(爆炸专用)(" + QHMod.AimKey1.ToString() + ")：</color></b>" + (QHMod.Aim1 ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>近战距离(" + QHMod.AttDistanceStateKey.ToString() + ")：</color></b>" + (QHMod.AttDistanceState ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>起飞开关(" + QHMod.SuperJumpTypeKey.ToString() + ")：</color></b>" + (QHMod.SuperJumpType ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>大头开关(" + QHMod.ZoomWeakStateKey.ToString() + ")：</color></b>" + (QHMod.ZoomWeakState ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>血条透视(" + QHMod.ShowBloodBarStateKey.ToString() + ")：</color></b>" + (QHMod.ShowBloodBarState ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>透视开关(" + QHMod.shownpcKey.ToString() + ")：</color></b>" + (QHMod.shownpc ? "开" : "关"), null);
                GUILayout.Label("<b><color=red>加入Discord群(PageDown键加入)</color></b>", null);
                GUILayout.Label("<b><color=red>"+ "项目地址(PageUp键打开项目):" + BuildInfo.DownloadLink.ToString() +"</color></b>", null);
                GUILayout.EndArea();
            }
            //起飞
            if (SuperJumpType && Input.GetKey(KeyCode.Space) && !HeroMoveManager.HMMJS.inputJump)
            {
                HeroCameraManager.HeroTran.Translate(Vector3.up * 0.3f);
            }
            //近战距离
            if (AttDistanceState)
            {
                foreach (KeyValuePair<int, WeaponPerformanceObj> weapon in HeroCameraManager.HeroObj.BulletPreFormCom.weapondict)
                {
                    weapon.value.WeaponAttr.AttDis = 9999f;
                    weapon.value.WeaponAttr.AttDistance = 9999f;
                    weapon.value.WeaponAttr.Radius = 9999f;
                }
            }
            //大头大头 下雨不愁
            if (ZoomWeakState)
            {
                List<NewPlayerObject> monsters = NewPlayerManager.GetMonsters();
                Transform weakTrans;
                foreach (var monster in monsters)
                {
                    try
                    {
                        weakTrans = monster.BodyPartCom.GetWeakTrans(false);
                    }
                    catch
                    {
                        continue;
                    }
                    weakTrans.localScale = new Vector3(ZoomWeakNum, ZoomWeakNum, ZoomWeakNum);
                }

            }
            //显示血条(透视怪)
            if (ShowBloodBarState)
            {
                try
                {
                    List<NewPlayerObject> monsters = NewPlayerManager.GetMonsters();
                    foreach (NewPlayerObject newPlayerObject in monsters)
                    {
                        if (newPlayerObject.BloodBarCom != null)
                        {
                            newPlayerObject.BloodBarCom.ShowBloodBar();
                        }
                    }
                }
                catch
                {
                    MelonLogger.Msg("OnGUIbug");
                }
            }
            if (shownpc)
            {
                try
                {
                    foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
                    {
                        NewPlayerObject value = keyValuePair.Value;
                        if (!(value.centerPointTrans == null) && ShowObject(value))
                        {
                            Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(value.centerPointTrans.transform.position);
                            if (vector.z > 0f)
                            {
                                string str = Vector3.Distance(HeroMoveManager.HeroObj.centerPointTrans.position, value.centerPointTrans.position).ToString("0.0");
                                GUI.Label(new Rect(vector.x, Screen.height - vector.y, 800f, 50f), FightTypeToString(value) + "(" + str + "m)");
                            }
                        }
                    }
                }
                catch
                {
                    MelonLogger.Msg("OnGUIbug");
                }
            }
        }

        public bool ShowObject(NewPlayerObject obj)
        {
            if (obj.FightType != ServerDefine.FightType.NWARRIOR_DROP_BULLET || obj.FightType != ServerDefine.FightType.NWARRIOR_DROP_CASH)
            {
                switch (obj.FightType)
                {
                    case ServerDefine.FightType.NWARRIOR_DROP_EQUIP: return true;

                    case ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL:
                        if (obj.Shape == 4406 || obj.Shape == 4419 || obj.Shape == 4427 || obj.Shape == 4430)
                            return true;
                        break;
                    case ServerDefine.FightType.NWARRIOR_NPC_TRANSFER:
                        if (obj.Shape == 4016 || obj.Shape == 4009 || obj.Shape == 4019 || obj.Shape == 4029)
                            return true;
                        break;
                    case ServerDefine.FightType.NWARRIOR_DROP_RELIC:
                    case ServerDefine.FightType.NWARRIOR_NPC_SMITH:
                    case ServerDefine.FightType.NWARRIOR_NPC_SHOP:
                    case ServerDefine.FightType.NWARRIOR_NPC_EVENT:
                    case ServerDefine.FightType.NWARRIOR_NPC_REFRESH:
                    case ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX:
                    case ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP:
                    case ServerDefine.FightType.NWARRIOR_NPC_PASSBOX:
                    case ServerDefine.FightType.NWARRIOR_NPC_MAGICBOX:
                    case ServerDefine.FightType.NWARRIOR_NPC_LOCKEDBOX:

                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }
        public string FightTypeToString(NewPlayerObject obj)
        {
            switch (obj.FightType)
            {
                case ServerDefine.FightType.NWARRIOR_DROP_EQUIP:
                    return DataMgr.GetWeaponData(obj.Shape).Name + " +" + obj.DropOPCom.WeaponInfo.SIProp.Grade.ToString();
                case ServerDefine.FightType.NWARRIOR_DROP_RELIC:
                    return DataMgr.GetRelicData(obj.DropOPCom.RelicSid).Name;
                case ServerDefine.FightType.NWARRIOR_NPC_SMITH:
                    return "工匠";
                case ServerDefine.FightType.NWARRIOR_NPC_SHOP:
                    return "商人";
                case ServerDefine.FightType.NWARRIOR_NPC_EVENT:
                case ServerDefine.FightType.NWARRIOR_NPC_REFRESH:
                    return "事件宝箱";
                case ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX:
                    return "奖励宝箱";
                case ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL:
                case ServerDefine.FightType.NWARRIOR_NPC_TRANSFER:
                    return "秘境";
                case ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP:
                    return "奇货商";
                case ServerDefine.FightType.NWARRIOR_NPC_PASSBOX:
                    return "过关宝箱";
                case ServerDefine.FightType.NWARRIOR_NPC_WEAPONSTORE:
                    return "武器商店";
                case ServerDefine.FightType.NWARRIOR_NPC_RELIC:
                    return "神器";
                case ServerDefine.FightType.NWARRIOR_NPC_TRANSFERPOS:
                    return "传送NPC";
                case ServerDefine.FightType.NWARRIOR_NPC_ROOMCHALLENGE:
                    return "房间挑战NPC";

                case ServerDefine.FightType.NWARRIOR_NPC_BENEDICTION:
                case ServerDefine.FightType.NWARRIOR_NPC_LIMITGOLDENCUP:
                case ServerDefine.FightType.NWARRIOR_NPC_CONNECTTRANSFER:
                case ServerDefine.FightType.NWARRIOR_NPC_RAREGOLDENCUP:
                default:
                    return obj.Shape.ToString();
            }
        }
        //随机数
        float GetRandomNumber(float minimum, float maximum)
        {
            return (UnityEngine.Random.value * (maximum - minimum) + minimum);
        }
    }
}
