using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TextUpdateTest
{

    private GameObject textUpdateObj;
    private TextUpdate textUpdate;
    private Text textComponent;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        textUpdateObj = new("Text Update");
        textUpdate = textUpdateObj.AddComponent<TextUpdate>();
        textComponent = textUpdateObj.AddComponent<Text>();

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestUpdateText()
    {
        Assert.True(textComponent.text.Equals(string.Empty), $"Actual: {textComponent.text}");

        textUpdate.UpdateTextValue(1f);

        Assert.True(textComponent.text.Equals(string.Format(textUpdate.format, 1f)), $"Actual: {textComponent.text}");

        yield return null;
    }
}