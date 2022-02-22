---
title: "OSC Overview"
excerpt: ""
slug: "osc-overview"
category: "620dac4c9751e40020be7447"
parent-doc: ""
gh-wiki-slug: "Home"
---

# Intro to OSC
[OSC](https://opensoundcontrol.stanford.edu/index.html) is a way to get different devices and applications to talk to each other. It's a favorite method of creative coders and people making weird interactive things because it's fast, networked, and very open-ended.

# What does this have to do with VRChat?
We're expanding OSC support in VRChat so you can control Avatars and Worlds in all sorts of new ways, and stream data out of VRChat to control other things, too! We're launching with two APIs: [Input](Input) and [Avatar Parameters](Avatar-Parameters). 

# How do I use it?
You can [enable it](#enabling-it) and then use an OSC-compatible application or device to stream data into VRChat, or to read data coming from it. This wiki is mostly full of info for people that want to _make these applications_. We'll add links to these sorts of apps after this info has been available for long enough for people to make them.

# VRChat Ports
For VRChat, we default to receiving on port 9000 and sending on port 9001. So an external program you use to send messages into VRChat should send to 9000, and if you want to listen to messages from VRChat, your application or device should listen to 9001. You can change these values with a command line arg: `--osc=inPort:senderIP:outPort`

If you were to replicate the default settings on the command line, they would be:
`--osc=9000:127.0.0.1:9001`

Note that you can also use 'localhost' instead of '127.0.0.1' if you like. If you wanted VRChat to send data to another device on the network, you would put that ip in the middle like this:
`--osc=9000:192.168.1.42:9001`

# OSC Tools
You'll need some OSC tools to test these features!

## Standalone Programs
We recommend [TouchOSC](https://hexler.net/touchosc) for sending (free Windows client), and [Protokol](https://hexler.net/protokol) for receiving. These tools have documentation on their site, please read through to understand how they work.

## Libraries
If you want to build your own from scratch, you have a few choices. 

If you're building something using C#, you can grab the [all-in-one branch of OscCore](https://github.com/vrchat/osccore/tree/all-in-one), which is the OSC lib VRChat uses internally.

If you're using Python, you can look into [python-osc](https://github.com/attwad/python-osc), which we have used for some internal testing with no issues.

# Enabling It
You can turn on OSC in the Action Menu under Osc > Enabled.
![enable-osc](https://user-images.githubusercontent.com/737888/154179201-ec413948-7013-494a-81fb-4b5e1129cf5f.jpg)

# Testing It
Check out the [Debugging](Debugging) page to see some incoming values whether or not they match up to anything.

# An Important Aside
Since interacting with VRChat's OSC APIs requires downloading programs from the internet, **exercise caution!** Ensure you are downloading a well-known, trustworthy application. Ask others who are using OSC to see what they're using.

If you have the expertise, consider writing your own implementations for your setup.