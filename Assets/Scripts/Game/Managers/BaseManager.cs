﻿using UnityEngine;
using System.Collections;
using Utils;


public abstract class BaseManager<T> : Singleton<T> where T : Component
{

    #region Variables

    [HideInInspector]
    public bool isReady { get; protected set; }

    #endregion

    #region Start

    protected override void Awake()
    {
        base.Awake();
        isReady = false;
    }

    void Start()
    {
        StartCoroutine(CoroutineStart());
        InitEvent();
    }

    protected abstract IEnumerator CoroutineStart();

    #endregion

    #region Initialized

    protected abstract void InitEvent();

    #endregion

}