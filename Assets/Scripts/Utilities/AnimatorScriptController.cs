using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

#if UNITY_EDITOR
[System.Serializable]
public abstract class BlendTreeMotion
{
    public AnimationClip animation;
    public float motionSpeed = 1f;
}

[System.Serializable]
public sealed class BlendTree1DMotion : BlendTreeMotion
{
    public float threshold;
}

[System.Serializable]
public sealed class BlendTree2DMotion : BlendTreeMotion
{
    public Vector2 position;
}

public class AnimatorScriptController
{
    public int workingLayer = 0;
    private readonly AnimatorController animatorController;
    public AnimatorScriptController(in AnimatorController animatorController) => this.animatorController = animatorController;

    public void AddState(string stateName, float stateSpeed, in Motion motion) 
    {
        AnimatorState state = animatorController.layers[workingLayer].stateMachine.AddState(stateName);
        state.speed = stateSpeed;
        state.motion = motion;
    }

    public void AddBlendTreeState(string stateName, float stateSpeed, in List<BlendTree2DMotion> motions, 
        string blendParameterX, string blendParameterY, BlendTreeType blendTreeType)
    {
        // Initialize Blend Tree
        BlendTree blendTree = new BlendTree();
        blendTree.name = "BlendTree";
        blendTree.blendType = blendTreeType;
        blendTree.blendParameter = blendParameterX;
        blendTree.blendParameterY = blendParameterY;

        // Set Blend Tree Children
        foreach (BlendTree2DMotion motion in motions)
            blendTree.AddChild(motion.animation);

        ChildMotion[] blendTreeChildren = blendTree.children;
        for (int i = 0; i < motions.Count; ++i) {
            blendTreeChildren[i].position = motions[i].position;
            blendTreeChildren[i].timeScale = motions[i].motionSpeed;
        }
        blendTree.children = blendTreeChildren;

        // Save blend tree so that it stays serialized
        AssetDatabase.AddObjectToAsset(blendTree, AssetDatabase.GetAssetPath(animatorController.layers[workingLayer].stateMachine));

        // Add State to state machine
        AddState(stateName, stateSpeed, blendTree);
    }
    public void AddBlendTreeState(string stateName, float stateSpeed, in List<BlendTree1DMotion> blendTree, 
        string blendParameter, BlendTreeType blendTreeType)
    {
        throw new System.NotImplementedException();
    }

    public void AddParameter(string parameter, AnimatorControllerParameterType parameterType) => animatorController.AddParameter(parameter, parameterType);

    public void AddAnyStateTransition(string destinationStateName,
        in AnimatorCondition[] conditions,
        bool canTransitionToSelf, float duration,
        TransitionInterruptionSource interruptionSource)
    {
        AnimatorState destinationState = animatorController.layers[workingLayer].stateMachine.states
            .Where(state => state.state.name == destinationStateName).Single().state;

        AnimatorStateTransition transition = animatorController.layers[workingLayer].stateMachine.AddAnyStateTransition(destinationState);

        foreach (AnimatorCondition condition in conditions)
            transition.AddCondition(condition.mode, condition.threshold, condition.parameter);
        transition.canTransitionToSelf = canTransitionToSelf;
        transition.duration = duration;
        transition.interruptionSource = interruptionSource;
    }

    /// <summary>
    /// Priority is position in AnyStateTransitions array. 0 is highest.
    /// </summary>
    public void AddAnyStateTransition(int priority,
        string destinationStateName,
        AnimatorCondition[] conditions,
        bool canTransitionToSelf, float duration,
        TransitionInterruptionSource interruptionSource)
    {
        List<AnimatorStateTransition> anyStateTransitions = new(animatorController.layers[workingLayer].stateMachine.anyStateTransitions);
        AddAnyStateTransition(destinationStateName, conditions, canTransitionToSelf, duration, interruptionSource);
        anyStateTransitions.Insert(priority,
            animatorController.layers[workingLayer].stateMachine.anyStateTransitions
            [animatorController.layers[workingLayer].stateMachine.anyStateTransitions.Length - 1]);
        animatorController.layers[workingLayer].stateMachine.anyStateTransitions = anyStateTransitions.ToArray();
    }
}
#endif
