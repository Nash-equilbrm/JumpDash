using Commons;
using DG.Tweening;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioSource bgAudio;
        public AudioSource sfxAudio;

        [Header("Background")]
        public AudioClip jumpDashBg;
        public AudioClip knifeHitBg;

        [Header("SFX")]
        public AudioClip btnClicked;
        public AudioClip loseGame;
        public AudioClip continueGame;

        public AudioClip playerHitBlock;
        public AudioClip playerJump;
        public AudioClip playerLand;
        public AudioClip playerThrowKnife;
        public AudioClip knifeHitTarget;
        public AudioClip knifeHitKnife;
        public AudioClip targetExplode;


        private void OnEnable()
        {
            StartCoroutine(IERegisterEvents());
        }


        private IEnumerator IERegisterEvents()
        {
            yield return new WaitUntil(() => PubSub.HasInstance);
            this.Register(EventID.OnPlayBtnClicked, PlayBgMusic);
            this.Register(EventID.LoseGame, OnLoseGame);
            this.Register(EventID.OnContinueClicked, OnContinueGame);
            this.Register(EventID.BtnClicked, (obj) => sfxAudio.PlayOneShot(btnClicked));
            this.Register(EventID.HitBlock, (obj) => sfxAudio.PlayOneShot(playerHitBlock));
            this.Register(EventID.HitObstacle, (obj) => sfxAudio.PlayOneShot(knifeHitKnife));
            this.Register(EventID.HitTarget, (obj) => sfxAudio.PlayOneShot(knifeHitTarget));
            this.Register(EventID.PlayerFinishLaunch, (obj) => sfxAudio.PlayOneShot(playerThrowKnife));
            this.Register(EventID.Jump, (obj) => sfxAudio.PlayOneShot(playerJump));
            this.Register(EventID.PlayerFinishMovement, (obj) => sfxAudio.PlayOneShot(playerLand));
            this.Register(EventID.TargetExplode, (obj) => sfxAudio.PlayOneShot(targetExplode));
        }

       

        private void OnDisable()
        {
            this.UnregisterAll(EventID.LoseGame);
            this.UnregisterAll(EventID.OnContinueClicked);

            this.UnregisterAll(EventID.OnPlayBtnClicked);
            this.UnregisterAll(EventID.BtnClicked);
            this.UnregisterAll(EventID.HitBlock);
            this.UnregisterAll(EventID.HitObstacle);
            this.UnregisterAll(EventID.HitTarget);
            this.UnregisterAll(EventID.PlayerFinishLaunch);
            this.UnregisterAll(EventID.Jump);
            this.UnregisterAll(EventID.PlayerFinishMovement);
            this.UnregisterAll(EventID.TargetExplode);
        }

        private void PlayBgMusic(object obj)
        {
            if (obj is not int gameIndex) return;
            var originalVolumn = bgAudio.volume;
            bgAudio.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                bgAudio.Stop();
                bgAudio.volume = originalVolumn;
                if (gameIndex == Constants.SCENE_JUMP_DASH)
                {
                    bgAudio.clip = jumpDashBg;
                }
                else if (gameIndex == Constants.SCENE_KNIFE_HIT)
                {
                    bgAudio.clip = knifeHitBg;
                }
                bgAudio.Play();
                bgAudio.loop = true;
            });
        }

        private void OnLoseGame(object obj)
        {
            var originalVolumn = bgAudio.volume;
            bgAudio.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                bgAudio.Stop();
                bgAudio.volume = originalVolumn;
                sfxAudio.PlayOneShot(loseGame);
            });
        }
        private void OnContinueGame(object obj)
        {
            bgAudio.Play();
            sfxAudio.PlayOneShot(continueGame);
        }
    }
}

