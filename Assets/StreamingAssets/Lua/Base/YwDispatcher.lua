--
-- Logic dispatcher class.
--
-- @filename  YwDispatcher.lua
-- @copyright Copyright (c) 2013 Yaukey all rights reserved.
-- @license   The MIT License (MIT)
-- @author    Yaukey
-- @date      2015-06-04
--

local YwRegister = require "Base/YwRegister"

local YwDeclare = YwDeclare
local YwClass = YwClass
local DLogError = YwDebug.LogError

-- Register new class.
local strClassName = "YwDispatcher"
local YwDispatcher = YwDeclare(strClassName, YwClass(strClassName, YwRegister))

-- The global instance.
YwDispatcher.m_cInstance = nil

-- Member variables.
function YwDispatcher:ctor()
    --print("YwDispatcher:ctor")

    if YwDispatcher.m_cInstance then
        DLogError("You have already create a YwDispatcher instance!")
        return
    end

    -- Assign the instance.
    YwDispatcher.m_cInstance = self
end

-- Destructor.
function YwDispatcher:dtor()
    --print("YwDispatcher:dtor")
    YwDispatcher.m_cInstance = nil
end

-- Static function.
-- Get the instance.
function YwDispatcher.Instance()
    --print("YwDispatcher:Instance")

    -- Check
    if not YwDispatcher.m_cInstance then
        YwDispatcher.new()
    end

    return YwDispatcher.m_cInstance
end

-- Return this class.
return YwDispatcher
