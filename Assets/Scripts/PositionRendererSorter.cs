﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour {

    private int sortingOrderBase = 5000; // This number should be higher than what any of your sprites will be on the position.y
    private int offset = 0;
    private bool runOnlyOnce = false;

    private float timer;
    private float timerMax = .1f;
    private Renderer myRenderer;

    private void Awake() {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    private void LateUpdate() {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            timer = timerMax;
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
            if (runOnlyOnce) {
                Destroy(this);
            }
        }
    }

    public void SetOffset(int offset) {
        this.offset = offset;
    }

}
