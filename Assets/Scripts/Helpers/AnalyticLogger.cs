using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.Scripts.Helpers;
using System.Linq;
using System.Resources;
using ResourceManager = Helpers.ResourceManager;

public static class AnalyticLogger
{
    private static List<AnalyticItem> _dataToSave = new List<AnalyticItem>();
    private static string _csvHeader = "TimeStamp;Event;Value";
    private static bool _addedHeader = false;
    private static string _defaultValue = "---";

    static AnalyticLogger()
    {
        /*_dataToSave.AddRange(new List<AnalyticItem>
        {
            new AnalyticItem
            {
                TimeStamp = DateTime.Now,
                Event = "Emotion to express",
                Value = "Smile"
            },
            new AnalyticItem
            {
                TimeStamp = DateTime.Now,
                Event = "Emotion registered",
                Value = "Neutral"
            },
            new AnalyticItem
            {
                TimeStamp = DateTime.Now,
                Event = "Emotion to express",
                Value = "Smile"
            },
            new AnalyticItem
            {
                TimeStamp = DateTime.Now,
                Event = "Emotion registered",
                Value = "Smile"
            }
        });*/
    }

    public static void AddData(AnalyticEventType type)
    {
        var item = new AnalyticItem
        {
            TimeStamp = DateTime.Now,
            Event = type.GetDescription(),
            Value = _defaultValue
        };

        AddData(item);
    }

    public static void AddData(AnalyticEventType type, string value)
    {
        var item = new AnalyticItem
        {
            TimeStamp = DateTime.Now,
            Event = type.GetDescription(),
            Value = value
        };

        AddData(item);
    }

    public static void AddData(string Event, string value)
    {
        var item = new AnalyticItem
        {
            TimeStamp = DateTime.Now,
            Event = Event,
            Value = value
        };

        AddData(item);
    }

    public static void AddData(AnalyticItem itemToAdd)
    {
        _dataToSave.Add(itemToAdd);
    }

    public static void SaveToFile(string filepathToSave = null)
    {
        var filepath = string.IsNullOrEmpty(filepathToSave) ? ResourceManager.FilepathWithAnalyticData : filepathToSave;

        var sb = new StringBuilder();

        if (!_addedHeader)
        {
            sb.AppendLine(_csvHeader);
            _addedHeader = true;
        }
        
        foreach (var item in _dataToSave)
        {
            sb.AppendLine(item.ToString());
        }

        try
        {
            File.AppendAllText(filepath, sb.ToString());
            ClearData();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public static void ClearData()
    {
        _dataToSave.Clear();
    }
}
