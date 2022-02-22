---
title: "OSC DIY"
excerpt: ""
slug: "osc-diy"
category: "620dac4c9751e40020be7447"
parent-doc: "620d7b41be3f830099d5a460"
gh-wiki-slug: "DIY"
---

You're welcome to make your own programs that communicate with VRChat over OSC. This takes some programming knowledge or learning - it's not too complicated, but it does deal with network connections and asynchronous messaging so it may not be a great first project.

We currently have two APIs available for interaction: Input and Avatar Parameters. Those pages will describe the messages available to send & receive and some other details.

# OSC Messages
An OSC message is made up of two main parts - an address, and a value. The address is slash-based and looks something like this: `/fridge/door/butter`. An address should always start with a slash, and you use slashes to separate categories of things. The value of a message can technically be any combination of any supported type - so it could be a single string, or it could be 3 floats, then a boolean, then a color, and finally a binary blob. **However** - the library we're using sacrifices this willy-nilly-ness in favor of performance. So we currently support values of type `int`, `float`, `bool` for Avatar Parameters, and can add others as needed.

OSC messaging works in a single-direction per-connection. There is a Sender and a Receiver. The receiver listens on a given port and the sender just blasts their UDP messages to that port, without knowing if anyone is there. You can start up the sender first or the receiver, it doesn't really matter - there's no handshakes involved.

# OSC Libraries
OSC is implemented in just about every programming language. 

For C#, we use [this branch of OscCore](https://github.com/vrchat/osccore/tree/all-in-one) within the VRChat client. It has the best performance and least memory usage of any C# library we found. 

If you're using Python, you can look into [python-osc](https://github.com/attwad/python-osc), which we have used for some internal testing with no issues.

Not a fan of these, or prefer a different language? [This page from the home of OSC](https://cnmat.org/OpenSoundControl/) has a great list of libraries.

# Async Gotchas
Since OSC is a network protocol, many implementations send and/or receive data on a background thread. In a Unity project, this means that you need to store this data and then apply it on the next frame update on the Main Thread. Each library handles this a little differently - if you're having an issue receiving and visualizing data, this is a good place to start poking.