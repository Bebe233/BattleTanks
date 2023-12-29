using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BEBE.Framework.Component
{
    public class BAnimation
    {
        protected bool isPlaying = false;

        protected int anim_frame_count = 0;
        protected Image m_image;
        protected Material m_material;
        protected Sprite[] m_key_frames;
        protected int m_frame_per_second = 12;
        protected int seconds_per_frame => 1 / m_frame_per_second;
        protected float timer = 0;
        public BAnimation(Material material, Sprite[] frames, int frame_per_second)
        {
            m_material = material;
            m_key_frames = frames;
            m_frame_per_second = frame_per_second;
            timer = 0;
        }

        public BAnimation(Image image, Sprite[] frames, int frame_per_second)
        {
            m_image = image;
            m_key_frames = frames;
            m_frame_per_second = frame_per_second;
            timer = 0;
        }

        public void DoUpdate(float deltaTime)
        {
            if (isPlaying)
            {
                timer += deltaTime;
                if (timer >= seconds_per_frame)
                {
                    timer -= seconds_per_frame;
                    var frame_play = m_key_frames[anim_frame_count++];
                    anim_frame_count %= m_key_frames.Length;
                    //m_material.SetTexture("_MainTex", frame_play);
                    m_image.sprite = frame_play;
                }
            }
        }

        public void DoPlay()
        {
            isPlaying = true;
        }

        public void DoPause()
        {
            isPlaying = false;
        }

        public void DoStop()
        {
            DoPause();
            do_reset();
        }

        protected void do_reset()
        {
            timer = 0;
            anim_frame_count = 0;
        }

    }

}
