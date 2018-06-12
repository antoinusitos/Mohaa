using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLog : MonoBehaviour
{
    public GameObject textLog = null;

    public Transform canvas = null;

    private int _index = 0;
    private int _indexMax = 13;
    private int _textSize = 15;

    private List<RectTransform> _allMessages = new List<RectTransform>();

    public void ShowLog(string log)
    {
        GameObject go = Instantiate(textLog, canvas);
        _index++;
        go.GetComponent<Text>().text = log;
        if (_index >= _indexMax)
        {
            _index--;
            Destroy(_allMessages[0].gameObject);
            _allMessages.RemoveAt(0);
        }
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(-50, -50, 0);
        _allMessages.Add(rect);
    }

    private void Update()
    {
        if (_allMessages.Count <= 0) return;

        int tempIndex = 0;
        for(int i = 0; i < _allMessages.Count; i++)
        {
            _allMessages[i].anchoredPosition = new Vector3(20, tempIndex * _textSize, 0);
            tempIndex--;
        }
    }
}
