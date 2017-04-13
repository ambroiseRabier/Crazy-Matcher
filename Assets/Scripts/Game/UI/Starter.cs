using UnityEngine;
using GAF.Core;
using GAFInternal.Core;
using Utils;
using System;
using System.Collections;

public delegate void StarterEventHandler(Starter sender);

[RequireComponent(typeof(GAFBakedMovieClip))]
public class Starter : Singleton<Starter> {
    private GAFBakedMovieClip m_anim;

    public event StarterEventHandler OnAnimationEnded;

    protected override void Awake () {
        base.Awake();
        m_anim = GetComponent<GAFBakedMovieClip>();
        m_anim.setAnimationWrapMode(GAFWrapMode.Once);
    }

    private void Start()
    {
        m_anim.gotoAndStop(m_anim.getFramesCount() - 1);
    }

    public void StartStarterThenPerform(Action action)
    {
        StartCoroutine(PlayAnimThenPerformOnEnd(action));
    }

    private IEnumerator PlayAnimThenPerformOnEnd(Action action)
    {
        m_anim.gotoAndPlay(0);

        while (m_anim.isPlaying())
            yield return null;

        if (OnAnimationEnded != null)
            OnAnimationEnded(this);

        action();
    }
}
