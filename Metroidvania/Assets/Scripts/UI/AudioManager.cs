using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	private int qSamples = 1024;
	private float refValue = 0.1f;
	private float threshold = 0.02f;
	public float rmsValue;
	private float dbValue;
	private float pitchValue;
	private float[] samples;
	private float[] spectrum;
	private float fSample;


	public AudioSource source;
	public AudioClip[] clips = new AudioClip[1];
	//public float[] samples = new float[audio.clip.samples * audio.clip.channels];
	public int play_index = 0;

	void Start()
	{
		source.clip = clips[play_index];
		source.Play();
		samples = new float[qSamples];
		spectrum = new float[qSamples];
		fSample = AudioSettings.outputSampleRate;

	}
	

	void Update()
	{
		AnalyzeRMS();
		AdjustPitch();
		//print ((rmsValue*1000).ToString());
	}


	void AdjustPitch()
	{
		source.pitch = Time.timeScale;
	}

	void AnalyzeRMS()
	{
		GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples
		int i;
		float sum = 0;
		for (i=0; i < qSamples; i++){
			sum += samples[i]*samples[i]; // sum squared samples
		}
		rmsValue = Mathf.Sqrt(sum/qSamples); // rms = square root of average
	}

	void AnalyzeSound()
	{
		GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples
		int i;
		float sum = 0;
		for (i=0; i < qSamples; i++){
			sum += samples[i]*samples[i]; // sum squared samples
		}
		rmsValue = Mathf.Sqrt(sum/qSamples); // rms = square root of average
		dbValue = 20*Mathf.Log10(rmsValue/refValue); // calculate dB
		if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
		// get sound spectrum
		GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		float maxV = 0;
		int maxN = 0;
		for (i=0; i < qSamples; i++){ // find max 
			if (spectrum[i] > maxV && spectrum[i] > threshold){
				maxV = spectrum[i];
				maxN = i; // maxN is the index of max
			}
		}
		float freqN = maxN; // pass the index to a float variable
		if (maxN > 0 && maxN < qSamples-1){ // interpolate index using neighbours
			var dL = spectrum[maxN-1]/spectrum[maxN];
			var dR = spectrum[maxN+1]/spectrum[maxN];
			freqN += 0.5f*(dR*dR - dL*dL);
		}
		pitchValue = freqN*(fSample/2)/qSamples; // convert index to frequency
	}
}
