using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandLayout : MonoBehaviour
{
    [SerializeField] private float _curveAngle;
    [SerializeField] private float _padding;
    [SerializeField] private float _shiftParameter;
    [SerializeField] private bool _reverse;
    private int _childCount
    {
        get
        {
            return transform.childCount;
        }
    }
    private int _reverseParam
    {
        get
        {
            return _reverse ? -1 : 1;
        }
    }
    private void OnTransformChildrenChanged()
    {
        _resetPositions();
    }

    private void _resetPositions()
    {
        float elementWidth;
        float cardAngle;
        int index;
        if (_childCount % 2 == 0)
        {
            foreach (Transform card in gameObject.transform)
            {
                elementWidth = card.gameObject.GetComponent<RectTransform>().rect.width;
                index = card.GetSiblingIndex();
                card.localPosition = new Vector3((elementWidth + _padding * _childCount) * (index - (_childCount / 2)), _reverseParam * _shiftParameter * (index - _childCount + 1) * index, 0);
                if (index < _childCount / 2)
                {
                    cardAngle = _reverseParam * _curveAngle * (index - (_childCount / 2));
                    card.localRotation = Quaternion.Euler(0, 0, cardAngle);
                }
                else
                {
                    cardAngle = _reverseParam * _curveAngle * (index - (_childCount / 2) + 1);
                    card.localRotation = Quaternion.Euler(0, 0, cardAngle);
                }
            }
        }
        else
        {
            foreach (Transform card in gameObject.transform)
            {
                elementWidth = card.gameObject.GetComponent<RectTransform>().rect.width;
                index = card.GetSiblingIndex();
                card.localPosition = new Vector3((elementWidth + _padding * _childCount) * (index - (_childCount / 2)), _shiftParameter * (index - _childCount + 1) * index, 0);
                if (index < _childCount / 2)
                {
                    cardAngle = _reverseParam * _curveAngle * (index - (_childCount / 2));
                    card.localRotation = Quaternion.Euler(0, 0, cardAngle);
                }
                else if (index == _childCount / 2)
                {
                    cardAngle = 0f;
                    card.localRotation = Quaternion.Euler(0, 0, cardAngle);
                }
                else if (index > _childCount / 2)
                {
                    cardAngle = _reverseParam * _curveAngle * (index - (_childCount / 2) + 1);
                    card.localRotation = Quaternion.Euler(0, 0, cardAngle);
                }
            }
        }
    }
}
