# Goal Icon Feature Implementation

## Overview
This document details the process of adding an icon field to the Goal model in the CommBank Goal Tracker application, including full implementation of emoji picker functionality for users to add and change goal icons.

## Project Context
- **Application**: CommBank Goal Tracker
- **Tech Stack**: React 17, TypeScript, Redux Toolkit, Styled Components
- **Backend API**: https://fencer-commbank.azurewebsites.net
- **Feature**: Add icon support to Goal model with emoji picker UI

## Implementation Process

### Step 1: Update Goal Model Definition

**File Modified**: `src/api/types.ts`

Added optional `icon` field to the Goal interface:

```typescript
export interface Goal {
  id: string
  name: string
  targetAmount: number
  balance: number
  targetDate: Date
  created: Date
  accountId: string
  transactionIds: string[]
  tagIds: string[]
  icon?: string
}
```

**Rationale**: Made the field optional (`icon?`) to maintain backward compatibility with existing goals that don't have icons.

### Step 2: Integrate Icon Management in Goal Manager

**File Modified**: `src/ui/features/goalmanager/GoalManager.tsx`

#### 2.1 Added Required Imports

```typescript
import { BaseEmoji } from 'emoji-mart'
import EmojiPicker from '../../components/EmojiPicker'
import AddIconButton from './AddIconButton'
import GoalIcon from './GoalIcon'
```

#### 2.2 Added State Management

Added state variables to track icon and emoji picker visibility:

```typescript
const [icon, setIcon] = useState<string | null>(null)
const [isEmojiPickerOpen, setIsEmojiPickerOpen] = useState<boolean>(false)
```

#### 2.3 Updated useEffect Hooks

Modified initialization effect to include icon:

```typescript
useEffect(() => {
  setName(props.goal.name)
  setTargetDate(props.goal.targetDate)
  setTargetAmount(props.goal.targetAmount)
  setIcon(props.goal.icon ?? null)
}, [
  props.goal.id,
  props.goal.name,
  props.goal.targetDate,
  props.goal.targetAmount,
  props.goal.icon,
])
```

Added effect to sync icon changes from Redux:

```typescript
useEffect(() => {
  setIcon(goal.icon ?? null)
}, [goal.icon])
```

#### 2.4 Implemented Handler Functions

**Toggle Emoji Picker**:
```typescript
const toggleEmojiPicker = (event: React.MouseEvent) => {
  event.stopPropagation()
  setIsEmojiPickerOpen(!isEmojiPickerOpen)
}
```

**Handle Emoji Selection**:
```typescript
const pickEmojiOnClick = (emoji: BaseEmoji, event: React.MouseEvent) => {
  event.stopPropagation()
  setIcon(emoji.native)
  setIsEmojiPickerOpen(false)
  const updatedGoal: Goal = {
    ...props.goal,
    name: name ?? props.goal.name,
    targetDate: targetDate ?? props.goal.targetDate,
    targetAmount: targetAmount ?? props.goal.targetAmount,
    icon: emoji.native,
  }
  dispatch(updateGoalRedux(updatedGoal))
  updateGoalApi(props.goal.id, updatedGoal)
}
```

#### 2.5 Updated UI Layout

Added icon management section at the top of the goal manager:

```typescript
<TopSection>
  <AddIconButtonContainer shouldShow={!icon}>
    <AddIconButton hasIcon={icon != null} onClick={toggleEmojiPicker} />
  </AddIconButtonContainer>

  <GoalIconContainer shouldShow={icon != null}>
    <GoalIcon icon={icon} onClick={toggleEmojiPicker} />
  </GoalIconContainer>

  <EmojiPickerContainer isOpen={isEmojiPickerOpen} hasIcon={icon != null}>
    <EmojiPicker onClick={pickEmojiOnClick} />
  </EmojiPickerContainer>
</TopSection>
```

#### 2.6 Added Styled Components

```typescript
const TopSection = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  position: relative;
  margin-bottom: 2rem;
`

const AddIconButtonContainer = styled.div<{ shouldShow: boolean }>`
  display: ${(props) => (props.shouldShow ? 'flex' : 'none')};
`

const GoalIconContainer = styled.div<{ shouldShow: boolean }>`
  display: ${(props) => (props.shouldShow ? 'flex' : 'none')};
`

const EmojiPickerContainer = styled.div<{ isOpen: boolean; hasIcon: boolean }>`
  display: ${(props) => (props.isOpen ? 'flex' : 'none')};
  position: absolute;
  top: ${(props) => (props.hasIcon ? '8rem' : '4rem')};
  left: 0;
  z-index: 1000;
`
```

#### 2.7 Removed Unused Type Definitions

Removed three unused type definitions that were causing compiler warnings:
- `AddIconButtonContainerProps`
- `GoalIconContainerProps`
- `EmojiPickerContainerProps`

These were replaced with inline type definitions in the styled components.

### Step 3: Display Icon on Goal Cards

**File Modified**: `src/ui/pages/Main/goals/GoalCard.tsx`

#### 3.1 Updated Card Rendering

Added conditional icon display:

```typescript
return (
  <Container key={goal.id} onClick={onClick}>
    {goal.icon && <Icon>{goal.icon}</Icon>}
    <TargetAmount>${goal.targetAmount}</TargetAmount>
    <TargetDate>{asLocaleDateString(goal.targetDate)}</TargetDate>
  </Container>
)
```

#### 3.2 Added Icon Styling

```typescript
const Icon = styled.span`
  font-size: 3rem;
  margin-bottom: 0.5rem;
`
```

Updated container to center icon with other content:

```typescript
const Container = styled(Card)`
  display: flex;
  flex-direction: column;
  min-height: 140px;
  min-width: 140px;
  width: 33%;
  cursor: pointer;
  margin-left: 2rem;
  margin-right: 2rem;
  border-radius: 2rem;
  align-items: center;
  justify-content: center;
`
```

## Supporting Components

The following components were already present in the codebase and were leveraged for this implementation:

### EmojiPicker Component
**File**: `src/ui/components/EmojiPicker.tsx`

Wraps the `emoji-mart` library with theme support:
- Respects light/dark mode from Redux store
- Hides preview and skin tone selector for cleaner UI
- Passes click events to parent component

### AddIconButton Component
**File**: `src/ui/features/goalmanager/AddIconButton.tsx`

Displays "Add icon" button with smiley face icon:
- Only visible when goal has no icon
- Triggers emoji picker on click

### GoalIcon Component
**File**: `src/ui/features/goalmanager/GoalIcon.tsx`

Displays the selected emoji icon:
- Large clickable icon (6rem font size)
- Opens emoji picker to change icon

## User Flow

### Adding an Icon
1. User opens a goal in the modal (clicks goal card)
2. Goal Manager displays "Add icon" button (if no icon exists)
3. User clicks "Add icon" button
4. Emoji picker overlay appears
5. User selects an emoji
6. Icon is saved to local state, Redux store, and backend API
7. Icon appears on the goal card

### Changing an Icon
1. User opens a goal that has an icon
2. User clicks the existing icon
3. Emoji picker overlay appears
4. User selects a new emoji
5. Icon is updated in local state, Redux store, and backend API
6. New icon appears on the goal card

## Data Flow

```
User Action (Click)
    ↓
GoalManager Handler (toggleEmojiPicker / pickEmojiOnClick)
    ↓
Local State Update (setIcon)
    ↓
Redux Store Update (dispatch updateGoalRedux)
    ↓
Backend API Update (updateGoalApi)
    ↓
Goal Card Re-renders (displays icon)
```

## Key Technical Decisions

### 1. Optional Field
Made `icon` optional to avoid breaking existing goals without icons.

### 2. String Type
Used `string` type to store the native emoji character, allowing for simple rendering without additional dependencies.

### 3. Conditional Rendering
Used conditional rendering (`{goal.icon && <Icon>{goal.icon}</Icon>}`) to gracefully handle goals without icons.

### 4. Event Propagation
Added `event.stopPropagation()` in handlers to prevent the modal from closing when interacting with the emoji picker.

### 5. Absolute Positioning
Positioned emoji picker absolutely within the TopSection to overlay other content without disrupting layout.

### 6. Z-Index Management
Set z-index to 1000 on emoji picker container to ensure it appears above other elements.

## Testing Considerations

### Manual Testing Checklist
- [ ] Create a new goal without an icon
- [ ] Add an icon to an existing goal
- [ ] Change an icon on a goal that already has one
- [ ] Verify icon displays on goal card
- [ ] Verify icon persists after page reload
- [ ] Test in light and dark modes
- [ ] Test responsive layout on mobile devices
- [ ] Verify clicking outside emoji picker closes it
- [ ] Verify API updates are successful

### Edge Cases Handled
- Goals without icons (backward compatibility)
- Clicking icon opens picker for editing
- Emoji picker closes after selection
- Event propagation prevented to avoid modal closing

## Files Modified Summary

| File | Purpose | Changes |
|------|---------|---------|
| `src/api/types.ts` | Type definitions | Added `icon?: string` to Goal interface |
| `src/ui/features/goalmanager/GoalManager.tsx` | Goal editing UI | Added icon state, handlers, UI components, and styling |
| `src/ui/pages/Main/goals/GoalCard.tsx` | Goal display | Added icon rendering and styling |

## Dependencies Used

- **emoji-mart**: Emoji picker library (already installed)
- **emoji-mart/css/emoji-mart.css**: Emoji picker styles
- **BaseEmoji**: TypeScript type from emoji-mart

## Compilation Results

### Before Implementation
```
WARNING in src\ui\features\goalmanager\GoalManager.tsx
  Line 114:6:  'AddIconButtonContainerProps' is defined but never used
  Line 115:6:  'GoalIconContainerProps' is defined but never used
  Line 116:6:  'EmojiPickerContainerProps' is defined but never used
```

### After Implementation
```
No linter errors found.
```

## Conclusion

The icon feature has been successfully implemented with:
- ✅ Goal model updated with optional icon field
- ✅ Full emoji picker integration in Goal Manager
- ✅ Icon display on goal cards
- ✅ Add and change icon functionality
- ✅ Theme support (light/dark mode)
- ✅ API synchronization
- ✅ No linting errors
- ✅ Backward compatibility maintained

The implementation leverages existing components and follows the established patterns in the codebase, ensuring consistency and maintainability.

