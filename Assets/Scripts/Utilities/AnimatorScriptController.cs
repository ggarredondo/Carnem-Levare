using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

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
        #if UNITY_EDITOR
        AnimatorState state = new AnimatorState();
        state.name = stateName;
        state.speed = stateSpeed;
        state.motion = motion;
        animatorController.layers[workingLayer].stateMachine.AddState(state, Vector2.zero);
        #endif
    }

    public void AddBlendTreeState(string stateName, float stateSpeed, in List<BlendTree2DMotion> motions, 
        string blendParameterX, string blendParameterY, BlendTreeType blendTreeType)
    {
        #if UNITY_EDITOR
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

        // Add State to state machine
        AddState(stateName, stateSpeed, blendTree);
        #endif
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
        #if UNITY_EDITOR
        AnimatorState destinationState = animatorController.layers[workingLayer].stateMachine.states
            .Where(state => state.state.name == destinationStateName).Single().state;

        AnimatorStateTransition transition = animatorController.layers[workingLayer].stateMachine.AddAnyStateTransition(destinationState);

        foreach (AnimatorCondition condition in conditions)
            transition.AddCondition(condition.mode, condition.threshold, condition.parameter);
        transition.canTransitionToSelf = canTransitionToSelf;
        transition.duration = duration;
        transition.interruptionSource = interruptionSource;
        #endif
    }

    public void AddAnyStateTransitionTopPriority(string destinationStateName,
        AnimatorCondition[] conditions,
        bool canTransitionToSelf, float duration,
        TransitionInterruptionSource interruptionSource)
    {
        #if UNITY_EDITOR
        // Get current anyStateTransitions and remove them from state machine
        AnimatorStateTransition[] anyStateTransitions = animatorController.layers[workingLayer].stateMachine.anyStateTransitions;

        foreach (AnimatorStateTransition anyStateTransition in animatorController.layers[workingLayer].stateMachine.anyStateTransitions)
            animatorController.layers[workingLayer].stateMachine.RemoveAnyStateTransition(anyStateTransition);

        // Add new transition to AnyState transitions (there are no other transitions, so it goes to the top of the list)
        AddAnyStateTransition(destinationStateName, conditions, canTransitionToSelf, duration, interruptionSource);

        // Re-add previously removed AnyState transitions
        foreach (AnimatorStateTransition anyStateTransition in anyStateTransitions)
            AddAnyStateTransition(anyStateTransition.destinationState.name,
                anyStateTransition.conditions,
                anyStateTransition.canTransitionToSelf,
                anyStateTransition.duration,
                anyStateTransition.interruptionSource);
        #endif
    }
}
