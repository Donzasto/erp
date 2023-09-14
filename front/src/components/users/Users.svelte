<script lang="ts">
	let notes: User[] = [];

	function getUsers() {
		fetch('/api/users')
			.then((response) => response.json())
			.then((data: User[]) => {
				notes = data;
			});
	}

	import { onMount } from 'svelte';

	onMount(async () => {
		getUsers();
	});
</script>

<h2>Admin panel</h2>

{#each notes as note}
	<p>
		<b> {note.login}, {note.login} </b>
	</p>
{/each}
