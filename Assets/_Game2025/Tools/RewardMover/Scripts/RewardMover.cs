using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RewardMover : MonoBehaviour
{
    [Header("Config"), SerializeField] private bool _playOnAwake;
    [SerializeField] private float _duration = 1.5f;
    [Header("Reference"), SerializeField] private RectTransform _arrow;

    private List<MultipleReward> _items;
    private MultipleReward _currentItem;

    public UnityEvent<MultipleReward> OnItemChanged;

    
    private void Awake()
    {
        _items = this.GetComponentsInChildren<MultipleReward>().ToList();

        if(_playOnAwake)
            Play();
    }



    public MultipleReward Pause()
    {
        _arrow.DOKill();
        var item = GetMinDistanceItem();
        if (_currentItem == item) return _currentItem;
        ScaleDown(_currentItem?.transform);
        _currentItem = item;
        OnItemChanged?.Invoke(_currentItem);
        ScaleUp(_currentItem.transform);

        return _currentItem;
    }



    public void Play()
    {
        _arrow.DOKill();
        var anchoredPos =  _arrow.anchoredPosition;
        _arrow.anchoredPosition = new Vector2(-355, anchoredPos.y);
        _arrow.DOAnchorPosX(355, _duration).SetEase(Ease.InOutQuad).SetLoops(-1, loopType: LoopType.Yoyo).OnUpdate(() =>
        {
            var item = GetMinDistanceItem();
            if (_currentItem != item)
            {
                ScaleDown(_currentItem?.transform);
                _currentItem = item;
                OnItemChanged?.Invoke(_currentItem);
                ScaleUp(_currentItem.transform);
            }
        });
    }


    public void ScaleUp(Transform item)
    {
        if (item == null)
            return;
        item.localScale = new Vector3(1, 1.2f, 1);

    }


    public void ScaleDown(Transform item)
    {
        if (item == null)
            return; 
        item.localScale = Vector3.one;
    }


    private MultipleReward GetMinDistanceItem()
    {
        var distance = Vector3.Distance(_items[0].transform.position, _arrow.position);
        var index = 0;
        for (int i = 1; i < _items.Count; i++)
        {
            var currentDistance = Vector3.Distance(_items[i].transform.position, _arrow.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                index = i;
            }
        }

        return _items[index];
    }
}
