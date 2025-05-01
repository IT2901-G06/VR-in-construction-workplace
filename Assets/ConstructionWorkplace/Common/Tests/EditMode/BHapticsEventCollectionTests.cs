using System.Linq;
using Bhaptics.SDK2;
using NUnit.Framework;

public class BHapticsEventCollectionTests
{
    [Test]
    public void AllLeft()
    {
        MotorEvent motorEventA = BhapticsEventCollection.AllLeft;
        MotorEvent motorEventB = new(PositionType.GloveL, new int[] { 1, 1, 1, 1, 1, 1 });
        Assert.True(motorEventA.MotorValues.SequenceEqual(motorEventB.MotorValues) && motorEventA.PositionType == motorEventB.PositionType);
    }

    [Test]
    public void AllRight()
    {
        MotorEvent motorEventA = BhapticsEventCollection.AllRight;
        MotorEvent motorEventB = new(PositionType.GloveR, new int[] { 1, 1, 1, 1, 1, 1 });
        Assert.True(motorEventA.MotorValues.SequenceEqual(motorEventB.MotorValues) && motorEventA.PositionType == motorEventB.PositionType);
    }

    [Test]
    public void VestAll()
    {
        MotorEvent motorEventA = BhapticsEventCollection.VestAll;
        MotorEvent motorEventB = new(PositionType.Vest, new int[] {
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,

            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,
        });
        Assert.True(motorEventA.MotorValues.SequenceEqual(motorEventB.MotorValues) && motorEventA.PositionType == motorEventB.PositionType);
    }
}
