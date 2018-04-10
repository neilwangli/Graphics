using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;


namespace UnityEditor.VFX.Operator
{
    [VFXInfo(category = "Constants")]
    class Epsilon : VFXOperator
    {
        override public string name { get { return "Epsilon (ε)"; } }

        protected override IEnumerable<VFXPropertyWithValue> outputProperties
        {
            get
            {
                yield return new VFXPropertyWithValue(new VFXProperty(typeof(float), "ε"));
            }
        }

        override protected VFXExpression[] BuildExpression(VFXExpression[] inputExpression)
        {
            return new[] { VFXOperatorUtility.EpsilonExpression[VFXValueType.Float] };
        }
    }
}
