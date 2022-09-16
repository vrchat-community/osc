---
title: "OSC Chatbox"
excerpt: ""
slug: "osc-chatbox"
category: "620dac4c9751e40020be7447"
parent-doc: "620d7b41be3f830099d5a460"
gh-wiki-slug: "Chatbox"
---

# Overview
The Chatbox provides an alternate way of communication that is as similar as possible to communicating with voice in VRChat. We want everyone to be able to talk with their friends in VRChat, but we also don’t want to turn VRChat into a “general chat”, text chatroom, or IM service. The chatbox is meant for ephemeral messages that allow people near each other to communicate, but disappear once they’re gone, just like voice.

## Addresses

`/chatbox/input` s b
This address can either send text to your Chatbox immediately, or fill in your Chatbox's input field and wait for you to confirm it. It takes a string and boolean as inputs. The string must be ASCII text, and is what will appear in the chatbox. A boolean of TRUE will send the text directly to your chatbox, and a boolean of FALSE will instead pop up the chatbox's entry UI, with the string already added.

`/chatbox/typing` b
This address can toggle the typing indicator on and off. TRUE for on, FALSE for off.