using UnityEngine;
using System.Collections;

using Canal.Unity;

[ExecuteInEditMode]
public class AnimatedBoxCollider : Behavior {

    [System.Serializable]
    public class FrameInfo
    {
        public Vector3 Center = Vector3.zero;
        public Vector3 Size = Vector3.one;
        public int FrameCount = 1;
        public bool Enabled = true;
    }

    public FrameInfo[] Frames;
    public BoxCollider BoxCollider;

    public void SetFrame(int frame)
    {
        if (Frames.Length == 0)
        {
            return;
        }
            
        int index = 0;
        while (frame > 0)
        {
            frame -= Frames[index].FrameCount;
            index++;
        }

        if (index >= Frames.Length)
        {
            index = Frames.Length - 1;
        }

        SetFrameInfo(index);
    }

    [Range(0, 1)]
    public float currentFrame;
    public void Update()
    {
        if (!Application.isPlaying)
        {
            if (Frames.Length > 0)
            {
                int frame = Mathf.RoundToInt(Frames.Length * currentFrame);
                this.SetFrameInfo(Mathf.RoundToInt(Frames.Length * currentFrame));
                currentFrame = frame / (float)Frames.Length;
            }
        }
    }

    private void SetFrameInfo(int infoIndex)
    {
        infoIndex = Mathf.Clamp(infoIndex, 0, Frames.Length - 1);

        FrameInfo frameInfo = Frames[infoIndex];
        BoxCollider.center = frameInfo.Center;
        BoxCollider.size = frameInfo.Size;
        BoxCollider.enabled = frameInfo.Enabled;
    }
}
