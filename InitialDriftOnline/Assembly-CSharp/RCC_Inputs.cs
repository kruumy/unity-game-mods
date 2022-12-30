using System;

[Serializable]
public class RCC_Inputs
{
	public float throttleInput;

	public float brakeInput;

	public float steerInput;

	public float clutchInput;

	public float handbrakeInput;

	public float boostInput;

	public int gearInput;

	public void SetInput(float _throttleInput, float _brakeInput, float _steerInput, float _clutchInput, float _handbrakeInput, float _boostInput)
	{
		throttleInput = _throttleInput;
		brakeInput = _brakeInput;
		steerInput = _steerInput;
		clutchInput = _clutchInput;
		handbrakeInput = _handbrakeInput;
		boostInput = _boostInput;
	}

	public void SetInput(float _throttleInput, float _brakeInput, float _steerInput, float _clutchInput, float _handbrakeInput)
	{
		throttleInput = _throttleInput;
		brakeInput = _brakeInput;
		steerInput = _steerInput;
		clutchInput = _clutchInput;
		handbrakeInput = _handbrakeInput;
	}

	public void SetInput(float _throttleInput, float _brakeInput, float _steerInput, float _handbrakeInput)
	{
		throttleInput = _throttleInput;
		brakeInput = _brakeInput;
		steerInput = _steerInput;
		handbrakeInput = _handbrakeInput;
	}

	public void SetInput(float _throttleInput, float _brakeInput, float _steerInput)
	{
		throttleInput = _throttleInput;
		brakeInput = _brakeInput;
		steerInput = _steerInput;
	}
}
