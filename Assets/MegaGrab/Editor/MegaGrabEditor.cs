using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MegaGrab))]
public class MegaGrabEditor : Editor
{
	public static EditorWindow GetMainGameView()
	{
		System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
		Type type = assembly.GetType("UnityEditor.GameView");
		EditorWindow gameview = EditorWindow.GetWindow(type);
		return gameview;
	}

    [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override void OnInspectorGUI()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
		MegaGrab mg = (MegaGrab)target;

		EditorGUIUtility.LookLikeControls();

		mg.SrcCamera = (Camera)EditorGUILayout.ObjectField("Source Camera", mg.SrcCamera, typeof(Camera), true);
		mg.GrabKey = (KeyCode)EditorGUILayout.EnumPopup("Grab Key", mg.GrabKey);
		mg.ResUpscale = EditorGUILayout.IntField("Res Upscale", mg.ResUpscale);
		mg.CalcFromSize = EditorGUILayout.BeginToggleGroup("Calc From Size", mg.CalcFromSize);
		mg.Dpi = EditorGUILayout.IntField("Dpi", mg.Dpi);
		mg.Width = EditorGUILayout.FloatField("Width", mg.Width);
		EditorGUILayout.EndToggleGroup();

		mg.OutputFormat = (IMGFormat)EditorGUILayout.EnumPopup("Output Format", mg.OutputFormat);

		if ( mg.OutputFormat == IMGFormat.Tga )
		{
			mg.alphagrab = EditorGUILayout.BeginToggleGroup("Alpha Grab", mg.alphagrab);
			mg.usemask = EditorGUILayout.Toggle("Use Mask", mg.usemask);
			mg.alphamaskcol = EditorGUILayout.ColorField("Mask Col", mg.alphamaskcol);
			EditorGUILayout.EndToggleGroup();
		}
		else
			mg.Quality = EditorGUILayout.Slider("Jpg Quality", mg.Quality, 0.0f, 100.0f);

		mg.uploadGrabs = EditorGUILayout.BeginToggleGroup("Upload Grab", mg.uploadGrabs);
		mg.m_URL = EditorGUILayout.TextField("Url", mg.m_URL);
		EditorGUILayout.EndToggleGroup();

		mg.sequenceGrab = EditorGUILayout.BeginToggleGroup("Sequence Grab", mg.sequenceGrab);
		mg.adddatetime = EditorGUILayout.Toggle("Add Date Time", mg.adddatetime);
		mg.CancelKey = (KeyCode)EditorGUILayout.EnumPopup("Cancel Key", mg.CancelKey);
		mg.framerate = EditorGUILayout.IntField("FrameRate", mg.framerate);
		mg.grabTime = EditorGUILayout.FloatField("Grab Time", mg.grabTime);

		EditorGUILayout.EndToggleGroup();

		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("Number of Grabs " + mg.NumberOfGrabs);
		EditorGUILayout.LabelField("Estimated Time " + mg.EstimatedTime + "s");

		EditorWindow gameView = GetMainGameView();
		Rect pos1 = gameView.position;

		if ( mg.CalcFromSize )
			mg.CalcUpscale((int)pos1.width, (int)pos1.height);
		else
		{
			mg.GrabWidthWillBe = (int)pos1.width * mg.ResUpscale;
			mg.GrabHeightWillBe = (int)pos1.height * mg.ResUpscale;
		}

		EditorGUILayout.LabelField("Grab Size " + mg.GrabWidthWillBe + " x " + mg.GrabHeightWillBe);
		EditorGUILayout.EndVertical();

		mg.UseCoroutine = EditorGUILayout.Toggle("Use Coroutine", mg.UseCoroutine);

		mg.Blur = EditorGUILayout.FloatField("Blur", mg.Blur);
		mg.AASamples = EditorGUILayout.IntField("AA Samples", mg.AASamples);
		mg.FilterMode = (AnisotropicFiltering)EditorGUILayout.EnumPopup("Filter Mode", mg.FilterMode);
		mg.UseJitter = EditorGUILayout.Toggle("Use Jitter", mg.UseJitter);

		mg.SaveName = EditorGUILayout.TextField("Save Name", mg.SaveName);
		mg.Format = EditorGUILayout.TextField("Format", mg.Format);
		mg.Enviro = EditorGUILayout.TextField("Enviroment Var", mg.Enviro);
		mg.Path = EditorGUILayout.TextField("Path", mg.Path);

		mg.UseDOF = EditorGUILayout.BeginToggleGroup("Use DOF", mg.UseDOF);
		mg.focalDistance = EditorGUILayout.FloatField("Focal Distance", mg.focalDistance);
		mg.totalSegments = EditorGUILayout.IntField("Total Segments", mg.totalSegments);
		mg.sampleRadius = EditorGUILayout.FloatField("Sample Radius", mg.sampleRadius);
		EditorGUILayout.EndToggleGroup();
		if ( GUI.changed )
		{
			mg.CalcEstimate();
			EditorUtility.SetDirty(target);
		}
	}
}
