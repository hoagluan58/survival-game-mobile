[System.AttributeUsage(System.AttributeTargets.Field)]
public class ValueLayoutAttribute : System.Attribute {
    public string keyLabel, value1Label, value2Label, value3Label, value4Label;
    // Widths are only for the data field, the label is auto-sized.
    public float  keyWidth, value1Width, value2Width, value3Width, value4Width;

    public string GetLabel (int index) {
        return index switch {
            0 => keyLabel,
            1 => value1Label,
            2 => value2Label,
            3 => value3Label,
            4 => value4Label,
            _ => ""
        };
    }

    public float GetWidth (int index) {
        return index switch {
            0 => keyWidth,
            1 => value1Width,
            2 => value2Width,
            3 => value3Width,
            4 => value4Width,
            _ => 0f
        };
    }

    public ValueLayoutAttribute () {
        keyLabel = value1Label = value2Label = value3Label = value4Label = string.Empty;
        keyWidth = value1Width = value2Width = value3Width = value4Width = 0f;
    }
}
