using UnityEngine;
using GAF.Core;
using GAFInternal.Core;
using Utils;
using System;

public delegate void StarterEventHandler(Starter sender);

[RequireComponent(typeof(GAFBakedMovieClip))]
public class Starter : Singleton<Starter> {
    private GAFBakedMovieClip m_anim;

    public event StarterEventHandler OnAnimationEnded;

    public void StartStarterThenPerform(Action action)
    {
        m_anim.gotoAndStop(0);

        StarterEventHandler callback = null;
        callback = (Starter starter) =>
        {
            action();
            OnAnimationEnded -= callback;
        };

        OnAnimationEnded += callback;

        m_anim.play();
    }

    protected override void Awake () {
        base.Awake();
        m_anim = GetComponent<GAFBakedMovieClip>();
        m_anim.addTrigger(GafClipOnAnimationEnded, m_anim.getFramesCount() - 1);
        gameObject.SetActive(false);
    }

    private void GafClipOnAnimationEnded(GAFBaseClip obj)
    {
        gameObject.SetActive(false);

        if (OnAnimationEnded != null)
            OnAnimationEnded(this);
    }

    private void OnDestroy()
    {
        m_anim.removeTrigger(GafClipOnAnimationEnded, m_anim.getFramesCount() - 1);
    }
}
